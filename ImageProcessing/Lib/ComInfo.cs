using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ComInfo
{
    public enum Pixel
    {
        B = 0,
        G,
        R,
        A,
        MAX,
    };

    public enum ImgType
    {
        EdgeDetection = 0,
        GrayScale,
        Binarization,
        GrayScale2Diff,
        ColorReversal,
        MAX,
    };

    public enum ImgDataType
    {
        Jpg = 0,
        Png,
        MAX,
    };

    public enum PictureType
    {
        Original = 0,
        After,
        MAX,
    };

    public enum ImageProcStatus
    {
        NotImplemented = 0,
        Implemented,
        MAX,
    }

    public const short RGB_MAX = 256;

    public const string MENU_SETTING_IMAGE_PROCESSING = "Image Processing";
    public const string IMG_NAME_EDGE_DETECTION = "EdgeDetection";
    public const string IMG_NAME_GRAY_SCALE = "GrayScale";
    public const string IMG_NAME_BINARIZATION = "Binarization";
    public const string IMG_NAME_GRAY_SCALE_2DIFF = "GrayScale 2Diff";
    public const string IMG_NAME_COLOR_REVERSAL = "ColorReversal";

    public const string IMG_TYPE_SELECT_NAME = "ImgTypeSelectName";
	public const string IMG_TYPE_EDGE_ID = "ImgTypeEdgeId";
    public const string IMG_TYPE_EDGE_NAME = "ImgTypeEdgeName";
    public const string IMG_TYPE_GRAY_SCALE_ID = "ImgTypeGrayScaleId";
    public const string IMG_TYPE_GRAY_SCALE_NAME = "ImgTypeGrayScaleName";
    public const string IMG_TYPE_BINARIZATION_ID = "ImgTypeBinarizationId";
    public const string IMG_TYPE_BINARIZATION_NAME = "ImgTypeBinarizationName";
    public const string IMG_TYPE_GRAY_SCALE_2DIFF_ID = "ImgTypeGrayScale2DiffId";
    public const string IMG_TYPE_GRAY_SCALE_2DIFF_NAME = "ImgTypeGrayScale2DiffName";
    public const string IMG_TYPE_COLOR_REVERSAL_ID = "ImgTypeColorReversalId";
    public const string IMG_TYPE_COLOR_REVERSAL_NAME = "ImgTypeColorReversalName";

    public const string RESOURCE_IMG_PROC_TYPE = "ImageProcessingType";
}
