using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using SwfDotNet.IO;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;
namespace LG.Utility {
    public class CreateVideoHelper {
        /// <summary>
        /// 生成flash
        /// </summary>
        /// <param name="imgFilePaths"></param>
        /// <param name="saveSwfFilePath"></param>
        /// <returns></returns>
        public static bool CreateSwf(List<string> imgFilePaths, string saveSwfFilePath) {
            try {
                Swf swf = new Swf();
                foreach (var item in imgFilePaths) {
                    if (!new FileInfo(item).Exists) continue;
                    Image img = Image.FromFile(item);
                    img = img.GetThumbnailImage(img.Width / 2, img.Height / 2, null, IntPtr.Zero);
                    int posX = 0;
                    int posY = 0;
                    int imgWidth = img.Width;
                    int imgHeight = img.Height;
                    //Create a new Swf instance
                    //Set size in inch unit (1 pixel = 20 inches)
                    swf.Size = new Rect(0, 0, (posX + imgWidth) * 20, (posY + imgHeight) * 20);
                    swf.Version = 7;  //Version 7 (for compression, must be > 5)
                    swf.Header.Signature = "CWS";  //Set the signature to compress the swf
                    //Set the background color tag as white
                    swf.Tags.Add(new SetBackgroundColorTag(255, 255, 255));
                    //Set the jpeg tag
                    ushort jpegId = swf.GetNewDefineId();
                    //Load the jped directly from an image
                    //In fact, this line will load the jpeg data in the file as 
                    //a library element only (not to display the jpeg)
                    swf.Tags.Add(DefineBitsJpeg2Tag.FromImage(jpegId, img));
                    //Now we will define the picture's shape tag
                    //to define all the transformations on the picture 
                    //(as rotation, color effects, etc..) 
                    DefineShapeTag shapeTag = new DefineShapeTag();
                    shapeTag.CharacterId = swf.GetNewDefineId();
                    shapeTag.Rect = new Rect(posX * 20 - 1, posY * 20 - 1,
                         (posX + imgWidth) * 20 - 1, (posY + imgHeight) * 20 - 1);
                    FillStyleCollection fillStyles = new FillStyleCollection();
                    fillStyles.Add(new BitmapFill(FillStyleType.ClippedBitmapFill,
                          ushort.MaxValue, new Matrix(0, 0, 20, 20)));
                    fillStyles.Add(new BitmapFill(FillStyleType.ClippedBitmapFill,
                                   jpegId, new Matrix(posX * 20 - 1, posY * 20 - 1,
                                   (20.0 * imgWidth) / img.Width,
                                   (20.0 * imgHeight) / img.Height)));
                    LineStyleCollection lineStyles = new LineStyleCollection();
                    ShapeRecordCollection shapes = new ShapeRecordCollection();
                    shapes.Add(new StyleChangeRecord(posX * 20 - 1, posY * 20 - 1, 2));
                    shapes.Add(new StraightEdgeRecord(imgWidth * 20, 0));
                    shapes.Add(new StraightEdgeRecord(0, imgHeight * 20));
                    shapes.Add(new StraightEdgeRecord(-imgWidth * 20, 0));
                    shapes.Add(new StraightEdgeRecord(0, -imgHeight * 20));
                    shapes.Add(new EndShapeRecord());
                    shapeTag.ShapeWithStyle =
                       new ShapeWithStyle(fillStyles, lineStyles, shapes);
                    swf.Tags.Add(shapeTag);

                    //Place the picture to the screen with depth=1
                    swf.Tags.Add(new PlaceObject2Tag(shapeTag.CharacterId, 1, 0, 0));
                    swf.Tags.Add(new ShowFrameTag());
                    swf.Tags.Add(new RemoveObject2Tag(1)); //THE ADDED LINE!!//
                    img.Dispose();
                }
                //Write the swf to a file
                SwfWriter writer = new SwfWriter(saveSwfFilePath);
                writer.Write(swf);
                writer.Close();
                return true;
            } catch {
                return false;
            }
        }
    }
}
