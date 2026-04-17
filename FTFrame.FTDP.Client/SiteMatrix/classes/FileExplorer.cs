using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;

namespace FTDPClient.Common
{
    /* Class  :FileExplorer
     * Date   : 10/03/2006
     * Discription : This class use to create the tree view and load 
     *               directories and files in to the tree
     *          
     */
    class FileExplorer
    {
        public FileExplorer()
        {

        }

        /* Method :CreateTree
         * Date   : 10/03/2006
         * Discription : This is use to creat and build the tree
         *          
         */

        public bool CreateTree(TreeView treeView)
        {
            bool returnValue = false;

            try
            {
                //// Create Desktop
                //TreeNode desktop = new TreeNode();
                //desktop.Text = "Desktop";
                //desktop.Tag = "Desktop";
                //desktop.Nodes.Add("");
                //treeView.Nodes.Add(desktop);
                // Get driveInfo
                TreeNode MyComputer = new TreeNode();
                MyComputer.Text = "My Computer";
                MyComputer.Tag = "My Computer";
                MyComputer.ImageIndex = 4;
                MyComputer.SelectedImageIndex = 4;
                treeView.Nodes.Add(MyComputer);
                foreach (DriveInfo drv in DriveInfo.GetDrives())
                {

                    TreeNode fChild = new TreeNode();
                    if (drv.DriveType == DriveType.CDRom)
                    {
                        fChild.ImageIndex = 0;
                        fChild.SelectedImageIndex = 0;
                    }
                    else if (drv.DriveType == DriveType.Fixed)
                    {
                        fChild.ImageIndex = 1;
                        fChild.SelectedImageIndex = 1;
                    }
                    else if (drv.DriveType == DriveType.Removable)
                    {
                        fChild.ImageIndex = 5;
                        fChild.SelectedImageIndex = 5;
                    }
                    fChild.Text = drv.Name;
                    fChild.Nodes.Add("");
                    MyComputer.Nodes.Add(fChild);
                    returnValue = true;
                }
                MyComputer.Expand();

            }
            catch
            {
                returnValue = false;
            }
            return returnValue;

        }

        /* Method :EnumerateDirectory
         * Date   : 10/03/2006
         * Discription : This is use to Enumerate directories and files
         *          
         */
        public TreeNode EnumerateDirectory(TreeNode parentNode)
        {

            try
            {
                DirectoryInfo rootDir;

                // To fill Desktop
                Char[] arr ={ '\\' };
                string FP = parentNode.FullPath.Substring(12);
                string[] nameList = FP.Split(arr);
                string path = "";

                if (nameList.GetValue(0).ToString() == "Desktop")
                {
                    path = SpecialDirectories.Desktop + "\\";

                    for (int i = 1; i < nameList.Length; i++)
                    {
                        path = path + nameList[i] + "\\";
                    }

                    rootDir = new DirectoryInfo(path);
                }
                // for other Directories
                else
                {

                    rootDir = new DirectoryInfo(FP + "\\");
                }

                parentNode.Nodes[0].Remove();
                foreach (DirectoryInfo dir in rootDir.GetDirectories())
                {

                    TreeNode node = new TreeNode();
                    node.Text = dir.Name;
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                    node.Nodes.Add("");
                    parentNode.Nodes.Add(node);
                }
                //Fill files
                foreach (FileInfo file in rootDir.GetFiles())
                {
                    TreeNode node = new TreeNode();
                    node.Text = file.Name;
                    switch(file.Extension.ToLower())
                    {
                        case ".gif":
                        case ".bmp":
                        case ".jpg":
                        case ".jpeg":
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                        case ".dll":
                            node.ImageIndex = 9;
                            node.SelectedImageIndex = 9;
                            break;
                        case ".xml":
                        case ".xsl":
                            node.ImageIndex = 6;
                            node.SelectedImageIndex = 6;
                            break;
                        case ".html":
                        case ".htm":
                            node.ImageIndex = 8;
                            node.SelectedImageIndex = 8;
                            break;
                        case ".txt":
                            node.ImageIndex = 11;
                            node.SelectedImageIndex = 11;
                            break;
                        case "":
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                            break;
                        default:
                            node.ImageIndex = 10;
                            node.SelectedImageIndex = 10;
                            break;
                    }
                    parentNode.Nodes.Add(node);
                }



            }

            catch
            {
                //TODO : 
            }

            return parentNode;
        }
    }
}
