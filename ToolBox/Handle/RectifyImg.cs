using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;

namespace ToolBox.Handle
{
    internal class RectifyImg
    {

        public Mat Rectify(string imgpath)
        {
            int[] imgSize = [488, 712];//w,h
            return Inference(imgpath, height: imgSize[1], width: imgSize[0]);
        }
        Mat Inference(string imgpath, int height, int width)
        {

            using Mat img = Cv2.ImRead(imgpath);
            //img = img.CvtColor(ColorConversionCodes.BGR2RGB);
            Cv2.CvtColor(img, img, ColorConversionCodes.BGR2RGB);

            Size size = new Size(width: width, height: height);

            using Mat resizedImg = new Mat();

            Cv2.Resize(img, resizedImg, size);


             var torchTensor = GetTorchTensorFromMat(resizedImg);
            //torch.permute(torchTensor, torchTensor, )
            torchTensor = torchTensor.permute(2, 0, 1);
            torchTensor.unsqueeze_(0);
            var data = GetDenseTensorFromTorchTensor(torchTensor);


            var input = NamedOnnxValue.CreateFromTensor("input_img", data);

            InferenceSession session = new InferenceSession("save.onnx");
            var point_positions2Ds = session.Run([input]);


            var point_positions2D = point_positions2Ds[0];//[1,2,45,31]

            var outTensor = point_positions2D.AsTensor<float>();

            var pointsTorchTensor = GetTorchTensorFromDenseTensor(outTensor);
            var unwarped = bilinear_unwarping(img, pointsTorchTensor);


            unwarped.squeeze_();//[3,h,w]=>[h,w,3] 1,2,0 
            unwarped = unwarped.permute([1, 2, 0]);
            unwarped *= 255;

            unwarped = unwarped.to_type(torch.uint8);

            Mat rgb = Mat.FromPixelData([img.Height, img.Width], MatType.CV_8UC3, unwarped.data<byte>().ToArray());
            return rgb.CvtColor(ColorConversionCodes.RGB2BGR);

        }

        DenseTensor<float> GetDenseTensorFromTorchTensor(torch.Tensor torchTensor)
        {
            //var dim = torchTensor.shape;
            var arr = torchTensor.data<float>().ToArray();
            var shape = torchTensor.shape;
            var dim = (from j in shape select (int)j).ToArray();
            DenseTensor<float> data = new DenseTensor<float>(arr, dim);
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="img_size">h,w</param>
        /// <returns></returns>
        torch.Tensor GetTorchTensorFromMat(Mat img)
        {
            img.ConvertTo(img, MatType.CV_32FC3, 1.0 / 255.0);
            var size = img.Size();
            float[] data = new float[size.Width * size.Height * img.Channels()];
            Marshal.Copy(img.Data, data, 0, data.Length);
            var t = torch.tensor(data, dtype: torch.float32);
            t = t.view([size.Height, size.Width, 3]);

            return t;
        }


        torch.Tensor bilinear_unwarping(Mat img, torch.Tensor point_positions)
        {

            var warped_img = GetTorchTensorFromMat(img);

            warped_img = warped_img.permute([2, 0, 1]);
            warped_img.unsqueeze_(0);
            var size = img.Size();

            var upsampled_grid = torch.nn.functional.interpolate(point_positions, size: [size.Height, size.Width], mode: torch.InterpolationMode.Bilinear, align_corners: true);
            var tmp = upsampled_grid.permute([0, 2, 3, 1]);

            var unwarped_img = torch.nn.functional.grid_sample(warped_img, tmp, align_corners: true);
            return unwarped_img;
        }
        torch.Tensor GetTorchTensorFromDenseTensor(Tensor<float> dtensor)
        {
            float[] arr = dtensor.ToArray();
            long[] dim = (from d in dtensor.Dimensions.ToArray() select (long)d).ToArray();
            var t = torch.tensor(arr, dtype: torch.float32);
            t = t.view(dim);
            return t;
        }
    }
}
