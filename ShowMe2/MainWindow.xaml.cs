using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphSharp.Controls;

namespace GraphSharpDemo
{

    public partial class MainWindow : Window
    {

        private MainWindowViewModel vm;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        public string s = "";
        public bool trav = false;


        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public MainWindow()
        {
           
            vm = new MainWindowViewModel();
            this.DataContext = vm;
            InitializeComponent();
            Panel1.Visibility = Visibility.Visible;
            Panel2.Visibility = Visibility.Hidden;
            Panel3.Visibility = Visibility.Hidden;
            textbox.IsEnabled = false;
            textbox2.IsEnabled = false;
            textbox3.IsEnabled = false;
            b1.IsEnabled = false;
            b2.IsEnabled = false;
            b3.IsEnabled = false;
            b4.IsEnabled = false;
            b5.IsEnabled = false;
            rsa.IsEnabled = false;


        }

        public void RSAEncrypt1(object sender, RoutedEventArgs e)
        {
            
            if (trav)
                try
                {
                    //Create a UnicodeEncoder to convert between byte array and string.
                    UnicodeEncoding ByteConverter = new UnicodeEncoding();
                    //Create byte arrays to hold original, encrypted, and decrypted data. 
                    byte[] dataToEncrypt = ByteConverter.GetBytes(s);
                    byte[] encryptedData;
                    byte[] decryptedData;
                    //Create a new instance of RSACryptoServiceProvider to generate 
                    //public and private key data. 
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {

                        //Pass the data to ENCRYPT, the public key information  
                        //(using RSACryptoServiceProvider.ExportParameters(false), 
                        //and a boolean flag specifying no OAEP padding.
                        encryptedData = vm.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
                        decryptedData = vm.RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

                        if (rsa.Content.ToString() == "RSAEncrypt")
                        {
                            MessageBox.Show("Encrypted Data: \n" + ByteConverter.GetString(encryptedData), "RSAEncrypt");
                            rsa.Content = "RSADecrypt";

                        }
                        else
                        {
                            MessageBox.Show("Decrypted Data: \n" + ByteConverter.GetString(decryptedData), "RSADecrypt");
                            rsa.Content = "RSAEncrypt";


                        }
                    }
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("Encryption failed.");
                }
            else
                MessageBox.Show("You have to Traverse the Data Structure first !");

            vm.path = "";
        }

        public void RSADecrypt1(object sender, RoutedEventArgs e)
        {
            
        }
        public void Button_Traverse(object sender, RoutedEventArgs e)
        {
                if ((ComboBox.Text == "Binary Search Tree") || (ComboBox.Text == "AVL"))
                {
                    try
                    {
                        if (trav)
                            s = "";
                        s = vm.preoreder.Substring(0, vm.preoreder.Length - 3);

                       
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {

                        MessageBox.Show("Empty Tree");
                    }
                    if (s != "")
                    {
                        MessageBox.Show(s, "PreOrder Traverse");
                        trav = true;
                    }
                }
                else if (ComboBox.Text == "Binary Tree")
                {
                    if (trav)
                        s = "";
                    if (vm.list.Count == 0)
                        MessageBox.Show("Empty Tree", "Tree Traverse");
                    else
                    {


                        for (int i = 0; i < vm.list.Count - 1; i++)
                            s += vm.list[i] + " -> ";
                        try
                        {
                            s += vm.list[vm.list.Count - 1];

                        }
                        catch (System.ArgumentOutOfRangeException)
                        {

                        }

                        MessageBox.Show(s, "Tree Traverse");
                    }
                    trav = true;

                }
                else if (ComboBox.Text == "Linked List")
                {
                    if (trav)
                        s = "";
                    if (vm.list.Count == 0)
                        MessageBox.Show("Empty List", "Tree Traverse");
                    else
                    {


                        for (int i = 0; i < vm.list.Count - 1; i++)
                            s += vm.list[i] + " -> ";
                        try
                        {
                            s += vm.list[vm.list.Count - 1];

                        }
                        catch (System.ArgumentOutOfRangeException)
                        {

                        }

                        MessageBox.Show(s, "Traverse");
                    }
                    trav = true;
                }
        }

        public void Button_Clickm(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void Button_Click44(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/forms/d/1rS-4X-HOHhOkuM7PgMZQHJBK8e7SF3SrTMbz58lQOmA/viewform?usp=send_form");
        }

       
      

        public void Button_Click6(object sender, RoutedEventArgs e)
        {
            try { vm.t0.Clear(); }
            catch (IndexOutOfRangeException) { }
            try { vm.t.Clear(); }
            catch (IndexOutOfRangeException) { }
            try { vm.G.Clear(); }
            catch (IndexOutOfRangeException) { }
            vm.preoreder = "";
            vm.path = "";
            vm.first = true;
            vm.firstGraph = true;
            vm.u = 0;
            vm.init();
            dfs_num.Text = "";
            textbox.Text = "";
            textbox1.Text = "";
            textbox2.Text = "";
            textbox3.Text = "";
            textbox10.Text = "";
            textbox11.Text = "";
        }

        public void Button_Click5(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Button_Click4(object sender, RoutedEventArgs e)
        {
            About a=new About();
            a.ShowDialog();
        }

        public void Button_Click3(object sender, RoutedEventArgs e)
        {
            bool found = false;
            if (textbox3.Text == "")
                MessageBox.Show("Enter a value !");
            else
            {
                try
                {
                    var v = Convert.ToInt32(textbox3.Text);

                }
                catch (FormatException)
                {
                    MessageBox.Show("Enter a positive interger number please !");
                    goto end;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Too big value !\nvalues must be less than " + "100000000");
                    goto end;
                }
                try
                {
                    if (Convert.ToInt32(textbox3.Text) > 100000000)
                    {
                        MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                        goto end;
                    }
                }
                catch (System.FormatException)
                {
                }
                for (int i = 0; i < vm.list.Count; i++)
                    if (vm.list[i] == Convert.ToInt32(textbox3.Text))
                    {
                        found = true;
                        break;
                    }
                if(found)
                    MessageBox.Show("Found !","Search");
                else
                    MessageBox.Show("Not Found !","Search");
              
            end: 
                textbox3.Text = "";

            }
        }

        public void Button_Click2(object sender, RoutedEventArgs e)
        {
            if ((ComboBox.Text == "Stack") || (ComboBox.Text == "Queue"))
            {
                 if (vm.list.Count <= 2)
                    MessageBox.Show("You can't delete if nodes count equals or less than two");
                 else
                vm.Delete(-1, ComboBox.Text);
            }
            else
            {
                bool found = false;
                if (textbox2.Text == "")
                    MessageBox.Show("Enter a value !");
                else if (vm.list.Count <= 2)
                {
                    MessageBox.Show("You can't delete if nodes count equals or less than two");
                    textbox2.Text = "";
                }
                else
                {
                    try
                    {
                        var v = Convert.ToInt32(textbox2.Text);

                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Enter a positive interger number please !");
                        goto end;
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                        goto end;
                    }
                    if (Convert.ToInt32(textbox2.Text) < 0)
                    {
                        MessageBox.Show("Enter a positive interger number please !");
                        goto end;
                    }
                    try
                    {
                        if (Convert.ToInt32(textbox2.Text) > 100000000)
                        {
                            MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                            goto end;
                        }
                    }
                    catch (System.FormatException)
                    {
                    }
                    for (int i = 0; i < vm.list.Count; i++)
                        if (vm.list[i] == Convert.ToInt32(textbox2.Text))
                        {
                            vm.preoreder = "";
                            vm.Delete(Convert.ToInt32(textbox2.Text), ComboBox.Text);
                            found = true;
                            break;
                        }
                    if (!found)
                        MessageBox.Show("Not Found !","Delete");


                    end:
                    textbox2.Text = "";

                }
            }
        }




        public void Button_Graph(object sender, RoutedEventArgs e)
        {
            if ((textbox10.Text == "") && (textbox11.Text == ""))
                MessageBox.Show("Enter the first and the second node");
            else if (textbox10.Text == "") 
                MessageBox.Show("Enter the first node");
            else if (textbox11.Text == "")
                MessageBox.Show("Enter the second node");         
            else
            {
                try
                {
                    var v1 = Convert.ToInt32(textbox10.Text);
                    var v2 = Convert.ToInt32(textbox11.Text);

                }
                catch (FormatException)
                {
                    MessageBox.Show("Enter a positive interger number please !");
                    goto end;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                    goto end;
                }
                try
                {
                    if (Convert.ToInt32(textbox10.Text) > 100000000)
                    {
                        MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                        goto end;
                    }
                }
                catch (System.FormatException)
                {
                }
                if ((Convert.ToInt32(textbox10.Text) < 0) || (Convert.ToInt32(textbox11.Text) < 0))
                {
                    MessageBox.Show("Enter a positive interger number please !");
                    goto end;
                }

                if (Convert.ToInt32(textbox10.Text) == Convert.ToInt32(textbox11.Text))
                   MessageBox.Show("you can't add an edge between the same node !");
                else
                vm.insert(Convert.ToInt32(textbox10.Text), Convert.ToInt32(textbox11.Text),ComboBox.Text);


            }



            end:
            vm.preoreder = "";
            textbox10.Text = "";
            textbox11.Text = "";

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool found = false;
            if ((textbox.Text == "") && (textbox1.Text == ""))
                    MessageBox.Show("Enter a value !");
                else
                {
                    try
                    {
                        var v = 1;
                        if ((ComboBox.Text == "Stack") || (ComboBox.Text == "Queue"))
                             v = Convert.ToInt32(textbox1.Text);
                        else
                             v = Convert.ToInt32(textbox.Text);

                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Enter a positive interger number please !");
                        goto end;
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Too big value !\nValues must be less than " +"100000000");
                        goto end;
                    }
                    try
                    {
                        if (Convert.ToInt32(textbox.Text) > 100000000)
                        {
                            MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                            goto end;
                        }
                   
                    }
                    catch (System.FormatException)
                    {
                        
                        
                    }
                      
                  
                    if (((textbox.Text != "") && (Convert.ToInt32(textbox.Text) < 0)) || ((textbox1.Text!="")&&(Convert.ToInt32(textbox1.Text) < 0)))
                    {
                        MessageBox.Show("Enter a positive interger number please !");
                        goto end;
                    }
                    if ((ComboBox.Text != "Stack") && (ComboBox.Text != "Queue"))
                    for (int i = 0; i < vm.list.Count; i++)
                        if (vm.list[i] == Convert.ToInt32(textbox.Text))
                        {
                            found = true;
                            break;
                        }
                    if (found && (ComboBox.Text != "Linked List"))
                        MessageBox.Show("This node is already in the Data Structure !","Insert");
                    else if (!found || (ComboBox.Text == "Linked List"))
                    {
                        vm.preoreder = "";
                        if ((ComboBox.Text != "Stack") && (ComboBox.Text != "Queue"))
                        vm.insert(Convert.ToInt32(textbox.Text),1, ComboBox.Text);
                        else
                            vm.insert(Convert.ToInt32(textbox1.Text),1, ComboBox.Text);

                    }
                }
                end:       
                textbox.Text = "";
                textbox1.Text = "";     
        }

        public void Button_t2(object sender, RoutedEventArgs e)
        {
            if ((dfs_num.Text == ""))
                MessageBox.Show("Enter a value !");
            else
            {
                try
                {
                    var v = 1;
                    v = Convert.ToInt32(dfs_num.Text);
                 

                }
                catch (FormatException)
                {
                    MessageBox.Show("Enter a positive interger number please !");
                    goto end;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                    goto end;
                }
                try
                {
                    if (Convert.ToInt32(textbox.Text) > 100000000)
                    {
                        MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                        goto end;
                    }

                }
                catch (System.FormatException)
                {
                }

             

                if (vm.list.Contains(Convert.ToInt32(dfs_num.Text)))
                {
                    if (trav)
                        s = "";
                    vm.DFS(vm.map[Convert.ToInt32(dfs_num.Text)]);
                     s = vm.path.Substring(0, vm.path.Length - 3);
                    MessageBox.Show(s,"DFS");
                    vm.visited = new bool[100];
                    vm.path = "";
                    trav = true;
                }
                else
                    MessageBox.Show("This node isn't in the Graph !");
            }

            end:
            dfs_num.Text = "";

        }

         public void Shortestpath(object sender, RoutedEventArgs e)
         {
             if ((textbox10.Text == "") && (textbox11.Text == ""))
                 MessageBox.Show("Enter the first and the second node");
             else if (textbox10.Text == "")
                 MessageBox.Show("Enter the first node");
             else if (textbox11.Text == "")
                 MessageBox.Show("Enter the second node");
             else
             {
                 try
                 {
                     var v1 = Convert.ToInt32(textbox10.Text);
                     var v2 = Convert.ToInt32(textbox11.Text);

                 }
                 catch (FormatException)
                 {
                     MessageBox.Show("Enter a positive interger number please !");
                     goto end;
                 }
                 catch (OverflowException)
                 {
                     MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                     goto end;
                 }
                 try
                 {
                     if (Convert.ToInt32(textbox10.Text) > 100000000)
                     {
                         MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                         goto end;
                     }
                 }
                 catch (System.FormatException)
                 {
                 }
                 if ((Convert.ToInt32(textbox10.Text) < 0) || (Convert.ToInt32(textbox11.Text) < 0))
                 {
                     MessageBox.Show("Enter a positive interger number please !");
                     goto end;
                 }


                 if (vm.list.Contains(Convert.ToInt32(textbox10.Text)) && vm.list.Contains(Convert.ToInt32(textbox11.Text)))
                 {
                     var v = vm.Shortest_BFS(vm.map[Convert.ToInt32(textbox10.Text)],
                         vm.map[Convert.ToInt32(textbox11.Text)]);
                     if (v != -1)
                     {
                         MessageBox.Show("Shortest Path Length is : " +
                                         v.ToString(),"Shortest Path");
                                        
                     }
                     else
                         MessageBox.Show("There is no path from node " + textbox10.Text + " to node " + textbox11.Text);

                     vm.visited = new bool[100];
                 }
                 else
                 {
                     if ((!vm.list.Contains(Convert.ToInt32(textbox10.Text)))&&(!vm.list.Contains(Convert.ToInt32(textbox11.Text))))
                           MessageBox.Show("Node " + textbox10.Text +" and node " +textbox11.Text +" aren't in the Graph !");
                     else if (!vm.list.Contains(Convert.ToInt32(textbox10.Text)))
                     MessageBox.Show("Node " + textbox10.Text + " isn't in the Graph !");
                       else if (!vm.list.Contains(Convert.ToInt32(textbox11.Text)))
                           MessageBox.Show("Node " + textbox11.Text + " isn't in the Graph !");
                       

                 }
             }

          

         end:
             vm.preoreder = "";
             textbox10.Text = "";
             textbox11.Text = "";
         }

   
        public void Button_t3(object sender, RoutedEventArgs e)
        {
            if ((dfs_num.Text == ""))
                MessageBox.Show("Enter a value !");
            else
            {
                try
                {
                    var v = 1;
                    v = Convert.ToInt32(dfs_num.Text);


                }
                catch (FormatException)
                {
                    MessageBox.Show("Enter a positive interger number please !");
                    goto end;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                    goto end;
                }
                try
                {
                    if (Convert.ToInt32(textbox.Text) > 100000000)
                    {
                        MessageBox.Show("Too big value !\nValues must be less than " + "100000000");
                        goto end;
                    }

                }
                catch (System.FormatException)
                {
                }



                if (vm.list.Contains(Convert.ToInt32(dfs_num.Text)))
                {
                    if (trav)
                        s = "";

                    vm.BFS(vm.map[Convert.ToInt32(dfs_num.Text)]);
                    s = vm.path.Substring(0, vm.path.Length - 3);
                    MessageBox.Show(s, "BFS");
                    vm.visited = new bool[100];
                    vm.path = "";
                    trav = true;
                }
                else
                    MessageBox.Show("This node isn't in the Graph !");
            }

        end:
            dfs_num.Text = "";
         
        }
    
 

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            vm.init();
            try { vm.t0.Clear(); }
            catch (IndexOutOfRangeException) { }
            try { vm.t.Clear(); }
            catch (IndexOutOfRangeException) { }
            Panel3.Visibility = Visibility.Hidden;
            vm.preoreder = "";
            vm.first = true;
            vm.firstGraph = true;
            if (ComboBox.Text != "Choose a Data Structure")
            {
                if (ComboBox.Text == "Stack" || ComboBox.Text == "Queue")
                    rsa.IsEnabled = false;
                else
                    rsa.IsEnabled = true;

                trav = false;
                s = "";
                textbox.IsEnabled = true;
                textbox2.IsEnabled = true;
                textbox3.IsEnabled = true;
                b1.IsEnabled = true;
                b2.IsEnabled = true;
                b3.IsEnabled = true;
                b4.IsEnabled = true;
                b5.IsEnabled = true;
            }
            if ((ComboBox.Text == "Stack") || (ComboBox.Text == "Queue"))
            {
                if (ComboBox.Text == "Stack")
                {
                    Button1.Content = "Push";
                    Button2.Content = "Pop";
                }
                else
                {
                    Button1.Content = "Enqueue";
                    Button2.Content = "Dequeue";
                }
                // button1.Content = "push";
                // ComboBox_Copy.Text = ComboBox.Text;
                Panel2.Visibility = Visibility.Visible;
            }
            else if (ComboBox.Text == "Graph")
            {
                Panel2.Visibility = Visibility.Hidden;
                Panel3.Visibility = Visibility.Visible;


            }
            else
            {

                Panel2.Visibility = Visibility.Hidden;
                Panel3.Visibility = Visibility.Hidden;
            }
           
        }

      

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //var hwnd = new WindowInteropHelper(this).Handle;
            //SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Button_peek(object sender, RoutedEventArgs e)
        {
            if (vm.list.Count == 0)
                MessageBox.Show(" Null !", "Peek", MessageBoxButton.OK);
            else
            {
                if (ComboBox.Text == "Stack")
                {
                    
                    MessageBox.Show("The top of the stack is : " + vm.list[vm.list.Count - 1].ToString(), "Peek",
                        MessageBoxButton.OK);

                }
                else
                {
                    MessageBox.Show("The top of the Queue is : " + vm.list[0].ToString(), "Peek",
                     MessageBoxButton.OK);
                }
            }
        }

        private void Window_Initialized(object sender, EventArgs e)        {        }
        private void ComboBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)  {        }
        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e){  ;   }
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)   {   }
        private void ComboBox_TouchLeave(object sender, TouchEventArgs e)    {  }

    }
}
