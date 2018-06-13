using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Collections;
using System.Windows.Controls.Ribbon;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        FileDialog fd = null;
        FileDialog fileToEncode = null;
        BitArray fileBits = null;
        BitArray extensionBits = null;
        BitArray decodedBits = null;
        private byte[] bitmapBytes = null;
        double widthOfBitmap;
        double heightOfBitmap;
        string level;
        string fileExtension;
        int fileSize;
        int rCapacity;
        int gCapacity;
        int bCapacity;

        private byte[] arrayOfBytesToEncode = null;
        private byte[] decodedBytes = null;

        byte[] tab = { 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F, 0x7F, 0xFF };
        byte[] tab2 = { 0xFE, 0xFC, 0xF8, 0xF0, 0xE0, 0xC0, 0x80, 0x00 };

        int bitsToSteal = 1;

        int rBitsToSteal;
        int gBitsToSteal;
        int bBitsToSteal;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenBitmap(object sender, RoutedEventArgs e)
        {
            try
            {
                fd = new OpenFileDialog();
                fd.Filter = "24-bit Bitmap (*.bmp)|*bmp";
                fd.ShowDialog();
                
                //BitmapImage b = new BitmapImage();
                //b.BeginInit();
                //b.UriSource = new Uri(fd.FileName);
                //b.EndInit();

                bitmapBytes = FileToByteArray(fd.FileName);

                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.StreamSource = new MemoryStream(bitmapBytes);
                b.EndInit();

                widthOfBitmap = b.PixelWidth;
                heightOfBitmap = b.PixelHeight;

                img1.Source = b;
                if (((b.Width - img1.Width) > 0) || ((b.Height - img1.Height) > 0))
                    img1.Stretch = Stretch.Uniform;
                else
                    img1.Stretch = Stretch.None;

                img1.Stretch = Stretch.Uniform;

                int temp = 0;
                for (int i = 1; i <= 4; i++)
                {
                    temp = bitmapBytes[i + 53] & 0x0F;
                    if (temp == i)
                        continue;
                    else
                        break;
                
                }
                if (temp == 4)
                    MessageBox.Show("Loaded bitmap contains encoded information!");

                Parameters.Visibility = Visibility.Visible;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
            }
            finally
            {
                fd = null;
            }

        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            try
            {
                fd = new OpenFileDialog();
                fd.Filter = "All files (*.*)|*";
                fd.ShowDialog();
                fileToEncode = fd;

                FileInfo fileInfo = new FileInfo(fd.FileName);
                fileExtension = fileInfo.Extension;
                //trim '.' from file extenstion
                fileExtension = fileExtension.Trim(new char[] { '.' });
                fileSize = (int)fileInfo.Length;

                //array of file extension bits
                //extensionBits = new BitArray(Encoding.ASCII.GetBytes(fileExtension));

                //array of file bits
                //fileBits = new BitArray(arrayOfBytesToEncode);

                //file to byte array
                arrayOfBytesToEncode = FileToByteArray(fd.FileName);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
            }
            finally
            {
                fd = null;
            }
        }
        
        private void SaveBitmap(object sender, RoutedEventArgs e)
        {
            try
            {
                fd = new SaveFileDialog
                {
                    Filter = "24-bit Bitmap (*.bmp)|*bmp"
                };
                fd.ShowDialog();
                //ChangeBitmap(ref bitmapBytes, c);
                //HideDataToBitmap(tablica);
                //HideDataToBitmapRGB(bitmapBytes);
                ByteArrayToFile(fd.FileName, bitmapBytes);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
            }
            finally
            {
                fd = null;
            }
            
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            try
            {
                decodedBytes = BitArrayToByteArray(decodedBits);
                fd = new SaveFileDialog();
                fd.ShowDialog();
                ByteArrayToFile(fd.FileName, decodedBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
            }

        }

        private void ChangeBitmap(ref byte[] bitmapData, int c)
        {
            Random rnd = new Random(DateTime.UtcNow.Millisecond);
            for (int i = 0xF57; i < 0xF5A; i++)
            {
                bitmapData[i] = (byte)rnd.Next(0x01, 0xFF);
                //i++;
                //bitmapData[i] = (byte)rnd.Next(0x01, 0xFF);
            }
            
        }

        private void HideDataToBitmap(byte[] bitmapBytes)
        {
            int numberOfBytesOfPadding = 0;

            if ((((widthOfBitmap + 3) / 4) % 1) == 0)
            {
                numberOfBytesOfPadding = 1;
            }
            else if ((((widthOfBitmap + 2) / 4) % 1) == 0)
            {
                numberOfBytesOfPadding = 2;
            }
            else if ((((widthOfBitmap + 1) / 4) % 1) == 0)
            {
                numberOfBytesOfPadding = 3;
            }
            else if (((widthOfBitmap / 4) % 1) == 0)
            {
                numberOfBytesOfPadding = 0;
            }

            int paddingBytes = 54;
            int x = 0;
            if(numberOfBytesOfPadding != 0)
            {
                for (int i = 0; i < heightOfBitmap; i++)
                {
                    paddingBytes += (3 * (int)widthOfBitmap) + numberOfBytesOfPadding * x;
                    x = 1;
                    for (int j = 0; j < numberOfBytesOfPadding; j++)
                    {
                        bitmapBytes[paddingBytes + j] = 0x23;
                    }
                }
            }

            FileStream fileToEncode = new FileStream("D:\\zad2\\test.txt", FileMode.Open);
            BinaryReader bytesToEncode = new BinaryReader(fileToEncode);
            arrayOfBytesToEncode = bytesToEncode.ReadBytes((int)fileToEncode.Length);
            fileToEncode.Dispose();
            fileToEncode.Close();
            Console.WriteLine(sizeof(bool));
            Console.ReadLine();
            int k = 0;
            BitArray bitArray = new BitArray(arrayOfBytesToEncode);
            int levelPosition = 53;
            if (level == "R")
            {
                //change R
                for (int j = 0; j < heightOfBitmap; j++)
                {

                    levelPosition += j * (int)widthOfBitmap * 3 + numberOfBytesOfPadding * j;
                    try
                    {
                        for (int i = 0; i < widthOfBitmap; i++)
                        {
                            byte temp = arrayOfBytesToEncode[k];
                            for (int n = 0; n < 8; n += bitsToSteal)
                            {
                                //if((8 - n) < bitsToSteal)
                                //{
                                //    levelPosition += 3;
                                //    int z = (arrayOfBytesToEncode[k] & tab[8 - n - 1]);
                                //    z <<= 8 - n - 1;
                                //    bitmapBytes[levelPosition] = (byte)((bitmapBytes[levelPosition] & tab2[8 - n]+1) + z);
                                //    arrayOfBytesToEncode[k] >>= 8 - n;
                                //    k++;
                                //    //levelPosition += 3;
                                //    bitmapBytes[levelPosition] = (byte)((bitmapBytes[levelPosition] & tab2[8 - n - 2]) + (arrayOfBytesToEncode[k] & tab[8 - n - 2]));
                                //    arrayOfBytesToEncode[k] >>= 8 - n - 1;
                                //    n = 8 - n - 1;
                                //    i++;
                                //}
                                try
                                {
                                    if (n + bitsToSteal > 8)
                                    {
                                        levelPosition += 3;
                                        //pozostala liczba bitow do ukrycia np. 3 ukryte, 3 ukryte i zostaja 2 przy 3 mozliwych do ukradniecia bitach
                                        int bitsToSteal_temp = 8 - n;
                                        bitmapBytes[levelPosition] = (byte)((bitmapBytes[levelPosition] & tab2[bitsToSteal_temp - 1]) + (temp & tab[bitsToSteal_temp - 1]));
                                        temp >>= bitsToSteal_temp;
                                        k++;
                                        if (k == arrayOfBytesToEncode.Length)
                                        {
                                            break;
                                        }
                                        i++;
                                        temp = arrayOfBytesToEncode[k];
                                        bitsToSteal_temp = bitsToSteal - bitsToSteal_temp;
                                        int mask_temp = 0xFF;
                                        int bts = bitsToSteal;
                                        int wyciaganyBit_temp = temp & tab[bitsToSteal_temp - 1];
                                        for (int m = 0; m < bitsToSteal_temp; m++)
                                        {
                                            mask_temp -= (int)Math.Pow(2, (bts - 1));
                                            bts--;
                                        }
                                        wyciaganyBit_temp <<= bts;
                                        bitmapBytes[levelPosition] = (byte)((bitmapBytes[levelPosition] & mask_temp) + (wyciaganyBit_temp));
                                        temp >>= bitsToSteal_temp;
                                        n = bitsToSteal_temp;
                                    }
                                    levelPosition += 3;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Kodowanie if wyjatek: " + ex.Message + ex.Data +ex.Source + ex.StackTrace);
                                }
                                
                                try
                                {
                                    // skladowa R & maska bitowa zerujaca bity skladowej przeznaczone na ukrycie danych + bajt ukrywanego pliku & maska do wyciagniecia ukrywanych bitow
                                    bitmapBytes[levelPosition] = (byte)((bitmapBytes[levelPosition] & tab2[bitsToSteal - 1]) + (temp & tab[bitsToSteal - 1]));
                                    temp >>= bitsToSteal;
                                    i++;
                                    //if(n > 8)
                                    //{
                                    //    int temp = n - 8;
                                    //    k++;
                                    //    bitmapBytes[levelPosition] = (byte)((bitmapBytes[levelPosition] & tab2[temp - 1]) + (arrayOfBytesToEncode[k] & tab[temp - 1]));
                                    //    arrayOfBytesToEncode[k] >>= temp;
                                    //    n = 0;
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Kodowanie zwykle wyjatek: " + ex.Message + ex.Data + ex.Source + ex.StackTrace);
                                }
                                
                            }
                            k++;
                            if (k >= arrayOfBytesToEncode.Length)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kodowanie petla z i wyjatek: " + ex.Message + ex.Data + ex.Source + ex.StackTrace);
                    }
                    
                    levelPosition = 53;
                    if (k >= arrayOfBytesToEncode.Length)
                    {
                        break;
                    }
                }

            }

            else if (level == "G")
            {
                //change G
                levelPosition = 52;
                for (int j = 0; j < heightOfBitmap; j++)
                {

                    levelPosition += j * (int)widthOfBitmap * 3 + numberOfBytesOfPadding * j;
                    for (int i = 0; i < widthOfBitmap; i++)
                    {
                        levelPosition += 3;
                        bitmapBytes[levelPosition] = 0xFE;
                    }
                    levelPosition = 52;
                }
            }

            else if (level == "B")
            {
                //change B
                levelPosition = 51;
                for (int j = 0; j < heightOfBitmap; j++)
                {

                    levelPosition += j * (int)widthOfBitmap * 3 + numberOfBytesOfPadding * j;
                    for (int i = 0; i < widthOfBitmap; i++)
                    {
                        levelPosition += 3;
                        bitmapBytes[levelPosition] = 0xFD;
                    }
                    levelPosition = 51;
                }
            }



            /*for (int i = 0; i < heightOfBitmap; i++)
            {
                for (int j = 0; j < widthOfBitmap; j++)
                {
                    rPosition += 3 + i * (int)widthOfBitmap *3;
                    t[rPosition] = 0xFF;
                }
                rPosition = 54;
            }*/

            k = 0;
            decodedBytes = new byte[13];
            for (int j = 0; j < heightOfBitmap; j++)
            {

                levelPosition += j * (int)widthOfBitmap * 3 + numberOfBytesOfPadding * j;
                for (int i = 0; i < widthOfBitmap; i++)
                {

                    for (int n = 0; n < 8 ; n+=bitsToSteal)
                    {
                        byte temp;
                        //if(n > 8)
                        //{
                        //    int tmp = 8 - (n - bitsToSteal);
                        //    temp = (byte)(bitmapBytes[levelPosition] & tab[tmp - 1]);
                        //    temp <<= tmp;
                        //    decodedBytes[k] += temp;
                        //    k++;
                        //    tmp = 8 - tmp;
                        //    temp = (byte)(bitmapBytes[levelPosition] & tab[tmp - 1]);
                        //    temp <<= tmp;
                        //    decodedBytes[k] += temp;
                        //    break;
                        //}
                        levelPosition += 3;
                        //decodedBytes[k] = (byte)(decodedBytes[k] | (bitmapBytes[levelPosition] & 1));
                        /*temp*/temp = (byte)(bitmapBytes[levelPosition] & tab[bitsToSteal-1]);
                        temp <<= n;
                        decodedBytes[k] += temp;
                        /*temp*///decodedBytes[k] <<= bitsToSteal;
                        
                        i++;
                    }
                    k++;
                    if (k == arrayOfBytesToEncode.Length)
                    {
                        break;
                    }
                }
                levelPosition = 53;
                if (k == arrayOfBytesToEncode.Length)
                {
                    break;
                }
            }

            FileStream example = new FileStream("D:\\zad2\\test2.txt", FileMode.OpenOrCreate);
            BinaryWriter example_wr = new BinaryWriter(example);
            example_wr.Write(decodedBytes);
            example.Close();

        }

        private void HideDataToBitmapRGB(byte[] bitmapBytes)
        {
            int numberOfBytesOfPadding = BytesOfPadding((int)widthOfBitmap);
            int levelPosition = 54;
            int temp = 0;
            int tmp = 0;
            int n = 0;

            fd = new OpenFileDialog();
            fd.Filter = "All files (*.*)|*";
            fd.ShowDialog();

            //file to byte array
            arrayOfBytesToEncode = FileToByteArray(fd.FileName);

            FileInfo fileInfo = new FileInfo(fd.FileName);
            fileExtension = fileInfo.Extension;
            //trim '.' from file extenstion
            fileExtension = fileExtension.Trim(new char[] { '.' });

            //array of file bits
            fileBits = new BitArray(arrayOfBytesToEncode);

            //array of extension bits
            extensionBits = new BitArray(Encoding.ASCII.GetBytes(fileExtension));



            //liczba bitow rozszerzenia + liczba bitow pliku + rozszerzenie + bity do ukrycia

            for (int i = 0; i < heightOfBitmap; i++)
            {
                //levelPosition += (int)widthOfBitmap * i + numberOfBytesOfPadding * i;
                
                for (int j = 0; j < widthOfBitmap * 3; j++)
                {

                    for (int k = 0; k < bBitsToSteal; k++)
                    {
                        tmp += Convert.ToInt32(fileBits[n]);
                        tmp <<= k;
                        temp += tmp;
                        n++;
                        tmp = 0;
                        if (n == fileBits.Length)
                            break;
                    }
                    bitmapBytes[levelPosition] = (byte)(bitmapBytes[levelPosition] & (tab2[bBitsToSteal - 1]));
                    bitmapBytes[levelPosition] += (byte)temp;
                    int x = bitmapBytes.Length;
                    tmp = 0;
                    temp = 0;
                    levelPosition++;
                    j++;
                    if (n == fileBits.Length)
                        break;

                    for (int k = 0; k < gBitsToSteal; k++)
                    {
                        tmp += Convert.ToInt32(fileBits[n]);
                        tmp <<= k;
                        temp += tmp;
                        n++;
                        tmp = 0;
                        if (n == fileBits.Length)
                            break;
                    }
                    bitmapBytes[levelPosition] = (byte)(bitmapBytes[levelPosition] & (tab2[gBitsToSteal - 1]));
                    bitmapBytes[levelPosition] += (byte)temp;
                    temp = 0;
                    tmp = 0;
                    levelPosition++;
                    j++;
                    if (n == fileBits.Length)
                        break;

                    for (int k = 0; k < rBitsToSteal; k++)
                    {
                        tmp += Convert.ToInt32(fileBits[n]);
                        tmp <<= k;
                        temp += tmp;
                        n++;
                        tmp = 0;
                        if (n == fileBits.Length)
                            break;
                    }
                    bitmapBytes[levelPosition] = (byte)(bitmapBytes[levelPosition] & (tab2[rBitsToSteal - 1]));
                    bitmapBytes[levelPosition] += (byte)temp;
                    temp = 0;
                    tmp = 0;
                    levelPosition++;
                    //j++;
                    if (n == fileBits.Length)
                        break;
                }
                levelPosition += numberOfBytesOfPadding;
                if (n == fileBits.Length)
                    break;
            }

            BitArray decodedBits = new BitArray(fileBits.Length);
            levelPosition = 54;
            int bitsOfByte = 0;
            n = 0;

            for (int i = 0; i < heightOfBitmap; i++)
            {
                //levelPosition += (int)widthOfBitmap * i + numberOfBytesOfPadding * i;
                for (int j = 0; j < widthOfBitmap * 3; j++)
                {

                    temp = (byte)(bitmapBytes[levelPosition] & (tab[bBitsToSteal - 1]));
                    levelPosition++;
                    j++;
                    for (int k = 0; k < bBitsToSteal; k++)
                    {
                        int temp2 = temp;
                        temp2 &= tab[k];
                        temp2 >>= k;
                        decodedBits.Set(n, Convert.ToBoolean(temp2));
                        n++;
                        if (n == fileBits.Length)
                            break;
                    }

                    temp = (byte)(bitmapBytes[levelPosition] & (tab[gBitsToSteal - 1])); ;
                    levelPosition++;
                    j++;
                    if (n == fileBits.Length)
                        break;
                    for (int k = 0; k < gBitsToSteal; k++)
                    {
                        int temp2 = temp;
                        temp2 &= tab[k];
                        temp2 >>= k;
                        decodedBits.Set(n, Convert.ToBoolean(temp2));
                        n++;
                        if (n == fileBits.Length)
                            break;
                    }

                    temp = (byte)(bitmapBytes[levelPosition] & (tab[rBitsToSteal - 1])); ;
                    levelPosition++;
                    //j++;
                    if (n == fileBits.Length)
                        break;
                    for (int k = 0; k < rBitsToSteal; k++)
                    {
                        int temp2 = temp;
                        temp2 &= tab[k];
                        temp2 >>= k;
                        decodedBits.Set(n, Convert.ToBoolean(temp2));
                        n++;
                        if (n == fileBits.Length)
                            break;
                    }
                    if (n == fileBits.Length)
                        break;
                }
                levelPosition += numberOfBytesOfPadding;
                if (n == fileBits.Length)
                    break;
            }

            decodedBytes = BitArrayToByteArray(decodedBits);
            fd = new SaveFileDialog();
            fd.ShowDialog();
            FileStream example = new FileStream(fd.FileName, FileMode.OpenOrCreate);
            BinaryWriter example_wr = new BinaryWriter(example);
            example_wr.Write(decodedBytes);
            example.Close();

        }

        /// <summary>
        /// Write file data to the byte array.
        /// </summary>
        /// <param name="path">Absolute path to the file.</param>
        /// <returns></returns>
        private static byte[] FileToByteArray(string path)
        {
            byte[] fileData = null;

            //open stream of file
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //file as binary data
            BinaryReader br = new BinaryReader(fs);
            //file binaries to byte array
            fileData = br.ReadBytes((int)fs.Length);
            //close stream
            fs.Close();
            //close reader
            br.Close();

            return fileData;
        }

        /// <summary>
        /// Write a byte array to file.
        /// </summary>
        /// <param name="path">Absolute path to the file.</param>
        /// <param name="tab">Byte array containing data to write to the file.</param>
        private static void ByteArrayToFile(string path, byte[] tab)
        {
            try
            {
                //open stream for file
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                //open binary writer
                BinaryWriter bw = new BinaryWriter(fs);
                //write bytes to file
                bw.Write(tab);
                fs.Close();
                bw.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Counting the number of bytes of padding appended to the end of the rows of 24-bit bitmap.
        /// </summary>
        /// <param name="width">Pixel width of bitmap</param>
        /// <returns></returns>
        private int BytesOfPadding(int width)
        {
            if ((((widthOfBitmap + 3) / 4) % 1) == 0)
            {
                return 1;
            }
            else if ((((widthOfBitmap + 2) / 4) % 1) == 0)
            {
                return 2;
            }
            else if ((((widthOfBitmap + 1) / 4) % 1) == 0)
            {
                return 3;
            }
            else if (((widthOfBitmap / 4) % 1) == 0)
            {
                return 0;
            }
            return -1;
        }

        private byte[] BitArrayToByteArray(BitArray bitArray)
        {
            byte[] bytes = new byte[bitArray.Length/8];
            bitArray.CopyTo(bytes, 0);
            return bytes;
        }

        private void Encode(object sender, RoutedEventArgs e)
        {
            try
            {
                int numberOfBytesOfPadding = BytesOfPadding((int)widthOfBitmap);
                int levelPosition = 69;//54;
                int n = 0;

                byte[] tempArray = new byte[4];
                //for (int i = 0; i < 4; i++)
                //{
                //    tempArray[i] = 0;
                //}
                //List<byte> lista = new List<byte>();

                //file extension
                //tempArray = Encoding.ASCII.GetBytes(fileExtension);
                //lista.AddRange(tempArray.ToList());
                //b bits
                //lista.Add((byte)bBitsToSteal);
                //g bits
                //lista.Add((byte)gBitsToSteal);
                //r bits
                //lista.Add((byte)rBitsToSteal);
                //file size
                tempArray = BitConverter.GetBytes(fileSize);
                //lista.AddRange(tempArray.ToList());
                //lista.AddRange(FileToByteArray(fileToEncode.FileName).ToList());
                byte temp;
                for (int i = 1; i <= 4; i++)
                {
                    temp = (byte)i;
                    bitmapBytes[i + 53] &= 0xF0;
                    bitmapBytes[i + 53] += temp;
                }

                temp = (byte)(bBitsToSteal & 0x0F);
                bitmapBytes[58] &= 0xF0;
                bitmapBytes[58] += temp;
                temp = (byte)(gBitsToSteal & 0x0F);
                bitmapBytes[59] &= 0xF0;
                bitmapBytes[59] += temp;
                temp = (byte)(rBitsToSteal & 0x0F);
                bitmapBytes[60] &= 0xF0;
                bitmapBytes[60] += temp;
                //arrayOfBytesToEncode = Concat(arrayOfBytesToEncode, tempArray);
                //arrayOfBytesToEncode = lista.ToArray();
                //arrayOfBytesToEncode = Concat(arrayOfBytesToEncode, FileToByteArray(fileToEncode.FileName));
                for (int i = 61; i < 69; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        temp = tempArray[j];
                        temp >>= 4;
                        bitmapBytes[i] &= 0xF0;
                        bitmapBytes[i] += temp;
                        i++;

                        temp = tempArray[j];
                        temp &= 0x0F;
                        bitmapBytes[i] &= 0xF0;
                        bitmapBytes[i] += temp;
                        i++;
                    }
                }
                fileBits = new BitArray(arrayOfBytesToEncode);

                for (int i = 0; i < heightOfBitmap; i++)
                {
                    for (int j = 0; j < widthOfBitmap * 3; j++)
                    {
                        HideBitsToBitmapByte(bBitsToSteal, ref levelPosition, ref n);
                        j++;
                        if (n == fileBits.Length) break;

                        HideBitsToBitmapByte(gBitsToSteal, ref levelPosition, ref n);
                        j++;
                        if (n == fileBits.Length) break;


                        HideBitsToBitmapByte(rBitsToSteal, ref levelPosition, ref n);
                        if (n == fileBits.Length) break;
                    }
                    levelPosition += numberOfBytesOfPadding;
                    if (n == fileBits.Length)
                        break;
                }
                

                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.StreamSource = new MemoryStream(bitmapBytes);
                b.EndInit();
                img2.Source = b;
                img2.Stretch = Stretch.Uniform;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
            }



        }
        private void Decode(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] tempArray = new byte[4];
                int levelPosition = 69;//54;
                int n = 0;
                int temp;

                int numberOfBytesOfPadding = BytesOfPadding((int)widthOfBitmap);
                bBitsToSteal = bitmapBytes[58] & 0x0F;
                gBitsToSteal = bitmapBytes[59] & 0x0F;
                rBitsToSteal = bitmapBytes[60] & 0x0F;

                slider_b.Value = bBitsToSteal;
                slider_g.Value = gBitsToSteal;
                slider_r.Value = rBitsToSteal;

                for (int i = 61; i < 69; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        temp = bitmapBytes[i] & 0x0F;
                        temp <<= 4;
                        i++;
                        temp += bitmapBytes[i] & 0x0F;
                        tempArray[j] = (byte)temp;
                        i++;
                    }
                }

                fileSize = BitConverter.ToInt32(tempArray, 0);

                decodedBits = new BitArray(fileSize * 8);

                for (int i = 0; i < heightOfBitmap; i++)
                {
                    //levelPosition += (int)widthOfBitmap * i + numberOfBytesOfPadding * i;
                    for (int j = 0; j < widthOfBitmap * 3; j++)
                    {
                        GetHiddenBitsFromBitmapByte(bBitsToSteal, ref levelPosition, ref n);
                        j++;
                        if (n == decodedBits.Length) break;

                        GetHiddenBitsFromBitmapByte(gBitsToSteal, ref levelPosition, ref n);
                        j++;
                        if (n == decodedBits.Length) break;

                        GetHiddenBitsFromBitmapByte(rBitsToSteal, ref levelPosition, ref n);
                        //j++;
                        if (n == decodedBits.Length) break;
                    }
                    levelPosition += numberOfBytesOfPadding;
                    if (n == decodedBits.Length)
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Data + ex.Source + ex.StackTrace);
            }

        }

        void HideBitsToBitmapByte(int dedicatedBits, ref int bitmapBytePosition, ref int fileBitPosition)
        {
            int temp1 = 0;
            int temp2 = 0;

            //Get number of bits (dedicatedBits) from bit array of file data (fileBits) 
            //which is set by the user for specified channel of RGB
            //and save them into temp1
            for (int k = 0; k < dedicatedBits; k++)
            {
                //get bit and add it to tmp
                temp1 += Convert.ToInt32(fileBits[fileBitPosition]);
                //bitwise right shift tmp
                //if we have to hide e.g. 010
                //first iteration: tmp = 0 temp = 0 (0)
                //second iteration: tmp = 1 temp = 2 (10)
                //third iteration: tmp = 0 temp = 2 (010)
                //this makes that particular bits are on the right position in byte
                temp1 <<= k;
                temp2 += temp1;
                //next bit in fileBits array
                fileBitPosition++;
                //resetting temp2 for next iteration
                temp1 = 0;
                if (fileBitPosition == fileBits.Length)
                    break;
            }
            //Clear (dedicatedBits) bits in bitmap byte where we hide bits from file  
            bitmapBytes[bitmapBytePosition] = (byte)(bitmapBytes[bitmapBytePosition] & (tab2[dedicatedBits - 1]));
            //Add 'bitsToHide' to bitmap byte
            bitmapBytes[bitmapBytePosition] += (byte)temp2;
            //next byte of bitmap
            bitmapBytePosition++;
        }

        void GetHiddenBitsFromBitmapByte(int dedicatedBits, ref int bitmapBytePosition, ref int encodedBitPosition)
        {
            //Get hidden bits from bitmap byte and save them to temp
            int temp = (byte)(bitmapBytes[bitmapBytePosition] & (tab[dedicatedBits - 1]));
            
            for (int k = 0; k < dedicatedBits; k++)
            {
                //copy of temp
                int temp2 = temp;
                //get consecutive bit from hidden bits
                //e.g. temp = 010 
                //first iteration temp2 = 0
                //second iteration temp2 = 1
                //third iteration temp2 = 0
                temp2 &= tab[k];
                //shift bit to LSB position
                temp2 >>= k;
                //save bit to array of encoded bits
                decodedBits.Set(encodedBitPosition, Convert.ToBoolean(temp2));
                //next position to save consecutive bit in encodedBits array
                encodedBitPosition++;
                //if (encodedBitPosition == fileBits.Length) break;
                if (encodedBitPosition == decodedBits.Length) break;
            }

            bitmapBytePosition++;
        }

        private byte[] Concat(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, c, 0, a.Length);
            Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }

        private void TestCapacity()
        {
            try
            {
                bCapacity = ((((int)widthOfBitmap * (int)heightOfBitmap) - 5) * 8) - ((((int)widthOfBitmap * (int)heightOfBitmap) - 5) * (int)slider_b.Value);
                bCapacity = (bCapacity * 8) / 1024;

                gCapacity = ((((int)widthOfBitmap * (int)heightOfBitmap) - 5) * 8) - ((((int)widthOfBitmap * (int)heightOfBitmap) - 5) * (int)slider_g.Value);
                gCapacity = (gCapacity * 8) / 1024;

                rCapacity = ((((int)widthOfBitmap * (int)heightOfBitmap) - 5) * 8) - ((((int)widthOfBitmap * (int)heightOfBitmap) - 5) * (int)slider_r.Value);
                rCapacity = (rCapacity * 8) / 1024;

                //int bitmapCapacity = bCapacity + gCapacity + rCapacity;

                //labelCapacity.Content = bitmapCapacity; 
            }
            catch (Exception)
            {

            }
        }

        private void AboutMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Icons comes from https://icons8.com/");
        }

        private void level_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox x = sender as TextBox;
            level = x.Text;
        }

        private void slider_r_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            bitsToSteal = (int)s.Value;
            rBitsToSteal = (int)s.Value;
            TestCapacity();
        }

        private void slider_g_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            gBitsToSteal = (int)s.Value;
            TestCapacity();
        }

        private void slider_b_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            bBitsToSteal = (int)s.Value;
            TestCapacity();
        }
    }

    class MyException : Exception
    {
        public string Wyjatek
        {
            get
            {
                return Message + "\n" + Source;
            }
        }
    }
}
