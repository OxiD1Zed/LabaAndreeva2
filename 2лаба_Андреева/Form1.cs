using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2лаба_Андреева
{
    public partial class Form1 : Form
    {
        List<int> arr = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonMin_Click(object sender, EventArgs e)
        {
            arr = stringArrToIntList(textBox1.Text.Trim().Split(' ', ',', '.'));
            if (arr.Count > 1)
                textBox2.Text = arr.Min().ToString();
            else
                MessageBox.Show("Массив не введен!");
        }

        private void buttonFileV_Click(object sender, EventArgs e)
        {
            arr = stringArrToIntList(textBox1.Text.Trim().Split(' ', ',', '.'));
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.txt";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                using (StreamWriter sw = fileInfo.AppendText())
                {
                    sw.WriteLine("\n" + textBox2.Text);
                }
            }
            else
            {
                MessageBox.Show("Файл не выбран!");
            }
        }

        private void buttonSeach_Click(object sender, EventArgs e)
        {
            arr = stringArrToIntList(textBox1.Text.Trim().Split(' ', ',', '.'));
            Stopwatch sw = new Stopwatch();
            List<int> indexs = new List<int>();
            Form3 form3 = new Form3();
            try
            {
                if (checkedListBox1.CheckedItems.Count < 1)
                    throw new Exception("Алгоритм поиска не выбран!");
                if (form3.ShowDialog() == DialogResult.OK)
                {
                    sw.Start();
                    string[] arrSeach = form3.GetTextBox1_Text().Trim().Split(' ', ',', '.');
                    if (checkedListBox1.GetItemChecked(0))
                    {
                        for (int i = 0; i < arrSeach.Length; i++)
                        {
                            indexs.Add(OrderSeach(arr, int.Parse(arrSeach[i])));
                        }
                    }
                    else if (checkedListBox1.GetItemChecked(1))
                    {
                        for (int i = 0; i < arrSeach.Length; i++)
                        {
                            indexs.Add(BinarySeach(arr, int.Parse(arrSeach[i])));
                        }
                    }
                    sw.Stop();
                    textBox2.Text = $"Искомые числа: {form3.GetTextBox1_Text()} \nИндексы искомых значений: " + String.Join(" ", indexs);
                }
                labelTime.Text = new TimeSpan(sw.ElapsedTicks).TotalMilliseconds + " мс.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonFileC_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(openFileDialog.FileName);
                using (StreamReader sr = new StreamReader(file.OpenRead()))
                {
                    textBox1.Text = sr.ReadToEnd().Trim();
                }
                arr = stringArrToIntList(textBox1.Text.Split(' ', ',', '.'));
            }
            else
            {
                MessageBox.Show("Файл не выбран!");
            }
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            try
            {
                arr.Clear();
                Random r = new Random();
                int size = r.Next(textBox3.Text != "" ? int.Parse(textBox3.Text) : 2,
                                  textBox4.Text != "" ? int.Parse(textBox4.Text) : 1000);
                for (int i = 0; i < size; i++)
                {
                    arr.Add(r.Next(10000));
                }
                textBox1.Text = String.Join(" ", arr);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            arr = stringArrToIntList(textBox1.Text.Trim().Split(' ', ',', '.'));
            if (arr.Count > 0)
                textBox2.Text = arr.Max().ToString();
            else
                MessageBox.Show("Массив не введен!");
        }

        private List<int> stringArrToIntList(string[] array)
        {
            List<int> tamp = new List<int>();
            try
            {
                for (int i = 0; i < array.Length; i++)
                {
                    tamp.Add(int.Parse(array[i]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return tamp;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = Program.RemoveCharString(textBox3.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = Program.RemoveCharString(textBox4.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = Program.RemoveCharString(textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackColor = Color.AntiqueWhite;
            Text = "Поиск в массиве";
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count > 1)
            {
                checkedListBox1.SetItemChecked(1 - checkedListBox1.SelectedIndex, false);
            }
        }

        private int BinarySeach(List<int> arrBinarySeach, int num)
        {
            int[] indexs = new int[arrBinarySeach.Count];
            for (int i = 0; i < indexs.Length; i++)
            {
                indexs[i] = i;
            }

            for (int i = 0; i < indexs.Length; i++)
            {

                for (int j = i + 1; j < indexs.Length; j++)
                {
                    if (arr[indexs[i]] > arr[indexs[j]])
                    {
                        int tamp = indexs[i];
                        indexs[i] = indexs[j];
                        indexs[j] = tamp;
                    }
                }
            }

            int low = 0;
            int high = arrBinarySeach.Count - 1;
            int midIndex;
            if (low == high)
                return -1;
            if (num < arrBinarySeach[indexs[low]] || num > arrBinarySeach[indexs[high]])
                return -1;

            while (true)
            {
                if(high - low == 1)
                {
                    if (arrBinarySeach[indexs[low]].CompareTo(num) == 0)
                        return indexs[low];
                    if (arrBinarySeach[indexs[high]].CompareTo(num) == 0)
                        return indexs[high];
                    return -1;
                }
                else
                {
                    midIndex = low + (high - low) / 2;
                    int comparisonResult = arrBinarySeach[indexs[midIndex]].CompareTo(num);
                    if (comparisonResult == 0)
                        return indexs[midIndex];
                    if(comparisonResult < 0)
                        low = midIndex;
                    if(comparisonResult > 0)
                        high = midIndex;
                }
            }
        }

        private int OrderSeach(List<int> arr, int num)
        {
            for(int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == num)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
