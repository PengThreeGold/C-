using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemSim
{
    public partial class MemoryCtrl : Form
    {
        public MemoryCtrl()
        {
            InitializeComponent();
        }

        private void MemoryCtrl_Load(object sender, EventArgs e)
        {
            ;
        }
        string algorithm = "None"; //�㷨ģʽ
        int lackcounts = 0;  //ȱҳ����
        int vpoint = 0;            //ҳ�����ָ��
        //int rpoint;            //ҳ���滻ָ��
        int pagelistlong;   //�������г���-1
        int lackflag = 1;           //ȱҳ��־��0Ϊ��ȱҳ��1Ϊȱҳ(Ĭ��ȱҳ)
        int[] pagelist = new int[20];     //��������
        string[] disk = new string[3];  //�洢�飨���浱ǰҳ�ţ�
        int[] wait = new int[3];    //�����ȴ�ʱ�䣨���ȼ���

        private void random_list_Click(object sender, EventArgs e)
        {
            disk1.Text = "";
            disk2.Text = "";
            disk3.Text = "";
            vpoint = 0; //�������з���ָ�����
            string str = "";    //��ʾ��������
            Random r = new Random(); //������������
            pagelistlong = r.Next(3,10) + 0;   //��������������и���[3,10]( +0 ���� ȷ��Ϊ�õ�int��)
            for(int i = 0; i <= pagelistlong; i++)
            {
                pagelist[i] = r.Next(1,10) + 1; //
                str += pagelist[i] + " ";
            }
            page_list.Text = str;
            richTextBox1.AppendText("������ɷ������г���Ϊ��"+(pagelistlong+1)+"\n");
        }
        //��ȡ�㷨ģʽ
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            algorithm = radioButton1.Text;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            algorithm = radioButton2.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            algorithm = radioButton3.Text;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            ;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (algorithm == "���ҳ���û��㷨��OPT��")
            {
                this.OPT();
            }else if (algorithm == "�Ƚ��ȳ�ҳ���û��㷨��FIFO��")
            {
                this.FIFO();
            }else this.LRU();
        }

        public void OPT()   //���ҳ���û��㷨��OPT��
        {
            //ͨ��ָ��λ���ж�����״̬
            if (vpoint > pagelistlong)
            {
                richTextBox1.AppendText("\nOPT�㷨��ȱҳ��Ϊ" + ((lackcounts) / (pagelistlong + 1.0)) + "\n" + "-----------------���н���--------------\n");
                timer1.Stop();  //ִ����˴ν���
                return; //�����˴�
            }

            int emptyflag = 0;  //�����Ϊ���źţ�0Ϊ�޿տ飬1Ϊ�пտ飩
            int emptydisk = -1; //�׸�Ϊ�յ������ţ�Ϊ-1��û��¼�յ������ţ�
            int max = 0;    //��Զҳ�����������ţ�0��1��2��
            wait[0] = 20; wait[1] = 20; wait[2] = 20;    //��ʼ���ȴ�ʱ��
            disk[0] = disk1.Text; disk[1] = disk2.Text; disk[2] = disk3.Text;   //��ȡ������鵱ǰҳ����Ϣ����

            //��ȱҳ��������У�
            for (int i = 0; i < 3; i++)
            {
                if (disk[i] == "")  //�ж��Ƿ���ڿ������
                {
                    emptyflag = 1;  //����п������
                    if (emptydisk == -1)    //�����һ��Ϊ�յ��������
                    {
                        emptydisk = i;
                    }
                }
                if (Convert.ToString(pagelist[vpoint]) == disk[i])  //�ж��Ƿ�ȱҳ
                {
                    lackflag = 0;   //��ʾ��ȱҳ
                    /* ע��
                     * �˴�Ҳ�ɲ�ʹ��ȱҳ�����
                     * ֱ�ӽ���return����
                     * Ϊ�˷����������ݽṹ������
                     * �˴�����ȱҳ�����
                     */
                    richTextBox1.AppendText("\n����ҳ��" + pagelist[vpoint] + "\n");
                    vpoint++;   //��������ָ��
                    break;
                }
            }

            //ȱҳ���
            if (emptyflag == 1 && lackflag == 1)    //���������Ϊ��ʱ
            {
                disk[emptydisk] = Convert.ToString(pagelist[vpoint]); //��ҳ����Ŵ���������
                disk1.Text = disk[0]; disk2.Text = disk[1]; disk3.Text = disk[2];//����������Ϣ����
                richTextBox1.AppendText("\n�����" + (emptydisk+1) + "���ڷ���ҳ��" + disk[emptydisk] + "\n");  //���ı�����ʾ�������
                vpoint++;    //��������ָ��
            }
            else if (emptyflag == 0 && lackflag == 1)   //����鲻Ϊ��ʱ
            {
                //�����ڿ�����顣�������ҳ�����ֵ
                for (int i = vpoint; i <= pagelistlong; i++)    //���滻ָ�뵽ҳ���б�βָ�����
                {
                    if (Convert.ToString(pagelist[i]) == disk[0] && wait[0] == 20)  //��¼���������ͬҳ��λ�ã�wait[0] == 20�ж��Ƿ�Ϊ�״μ�¼��
                    {
                        wait[0] = i - vpoint + 1;
                    }
                    else if (Convert.ToString(pagelist[i]) == disk[1] && wait[1] == 20) //��¼���������ͬҳ��λ��
                    {
                        wait[1] = i - vpoint + 1;
                    }
                    else if (Convert.ToString(pagelist[i]) == disk[2] && wait[2] == 20) //��¼���������ͬҳ��λ��
                    {
                        wait[2] = i - vpoint + 1;
                    }
                }
                for (int i = 0; i < 3; i++) //������Զҳ������������
                {
                    if (wait[i] > wait[max])
                    {
                        max = i;
                    }
                }
                richTextBox1.AppendText("\n����� 1 ��ҳ�滹��ȴ� " + wait[0] + "s ����\n" + "����� 2 ��ҳ�滹��ȴ� " + wait[1] + "s ����\n" + "����� 3 ��ҳ�滹��ȴ� " + wait[2] + "s ����\n");
                //�û�������Զ������ҳ��
                /* ע��
                 * �˴����Բ������ת����
                 * �滻ִ�д��򼴿ɣ�����->�������Ϊ�����->���ģ�
                 */
                string tempdisk = disk[max];    //������ת����
                disk[max] = Convert.ToString(pagelist[vpoint]); //ִ���滻����
                richTextBox1.AppendText("\n����� " + (max + 1) + " ��ҳ��" + tempdisk + "�û�Ϊҳ��" + disk[max] + "\n");
                disk1.Text = disk[0]; disk2.Text = disk[1]; disk3.Text = disk[2];   //����������Ϣ����
                lackflag = 1;   //����ȱҳ��־��Ĭ��ȱҳ��
                vpoint++;   //����ָ�����
            }
            
            if (lackflag == 1)  //����ȱҳ����
            {
                lackcounts++;   //ȱҳ������1
            }
            lackflag = 1;   //����ȱҳ��־��Ĭ��ȱҳ��
        }
        public void FIFO()  //�Ƚ��ȳ�ҳ���û��㷨��FIFO��
        {
            int emptyflag = 0;  //�����Ϊ���ź�
            int emptydisk = -1; //�׸�Ϊ�յ�������
            //ͨ��ָ��λ���ж�����״̬
            if (vpoint > pagelistlong)
            {
                richTextBox1.AppendText("FIFO�㷨��ȱҳ��Ϊ" + ((lackcounts) / (pagelistlong + 1.0)) + "\n" + "-----------------���н���--------------\n");
                timer1.Stop();  //ִ����˴ν���
                return; //�����˴�
            }
            richTextBox1.AppendText("����� 1 �ѵȴ�" + wait[0] + "\n����� 2 �ѵȴ�" + wait[1] + "\n����� 3 �ѵȴ�" + wait[2] + "\n");
            disk[0] = disk1.Text;disk[1] = disk2.Text;disk[2] = disk3.Text;//��ȡ�������Ϣ����
            //��ȱҳ��������У�
            for (int i = 0; i < 3; i++)
            {
                if (disk[i] == "")
                {
                    emptyflag = 1;  //����������Ϊ��
                    if (emptydisk == -1)
                    {
                        emptydisk = i;  //�����һ��Ϊ�յ��������
                    }
                }
                if (Convert.ToString(pagelist[vpoint]) == disk[i])
                {
                    lackflag = 0;   //��ʾ��ȱҳ
                    //wait[0]; wait[1]; wait[2];   //���������ȴ�ʱ��(����ĩβ��Ҫͳһ��һ)
                    richTextBox1.AppendText("����ҳ��" + pagelist[vpoint] + "�����������ȴ�ʱ�䲻��" + "\n");
                    vpoint++;   //��������ָ��
                    break;
                }
            }
            //ȱҳ���
            if (emptyflag == 1 && lackflag == 1)    //���������Ϊ��ʱ
            {
                disk[emptydisk] = Convert.ToString(pagelist[vpoint]); //��ҳ����Ŵ���������
                disk1.Text = disk[0];disk2.Text = disk[1];disk3.Text = disk[2];//����������Ϣ����
                richTextBox1.AppendText("�����" + (emptydisk+1) + "���ڷ���ҳ��" + disk[emptydisk] + "\n");  //���ı�����ʾ�������
                vpoint++;    //��������ָ��
            }
            else if (emptyflag == 0 && lackflag == 1)
            {
                //��ȡ�ȴ���������
                int max = 0;
                for (int i = 0; i < wait.Length; i++)
                {

                    if (wait[i] > wait[max])
                    {
                        max = i;
                    }
                }
                //�û��ȴ����������ҳ��
                string tempdisk = disk[max];
                disk[max] = Convert.ToString(pagelist[vpoint]);
                richTextBox1.AppendText("�����" + (max+1) + "��ҳ��" + tempdisk + "�û�Ϊҳ��" + disk[max] + "\n");
                disk1.Text = disk[0];disk2.Text = disk[1];disk3.Text = disk[2];//����������Ϣ����
                lackflag = 1;   //��ʾȱҳ
                wait[max] = 0; //���������ȴ�ʱ��
                vpoint++;   //��������ָ��
            }
            //���������Ѵ�ҳ�棩�����ȴ�ʱ�䣨ȱҳ����£�
            for (int i = 0; i < 3; i++)
            {
                if (disk[i] != "" && lackflag == 1)
                {
                    wait[i]++;
                }
            }
            if (lackflag == 1)
            {
                lackcounts++;   //ȱҳ������1
            }
            lackflag = 1;   //����ȱҳ��־��Ĭ��ȱҳ��
        }

        public void LRU()   //������δʹ���㷨
        {
            int emptyflag = 0;  //�����Ϊ���ź�
            int emptydisk = -1; //�׸�Ϊ�յ�������
            //ͨ��ָ��λ���ж�����״̬
            if (vpoint > pagelistlong)
            {
                richTextBox1.AppendText("LRU�㷨��ȱҳ��Ϊ"+( (lackcounts) / (pagelistlong + 1.0)) +"\n" + "-----------------���н���--------------\n");
                timer1.Stop();  //ִ����˴ν���
                return; //�����˴�
            }
            richTextBox1.AppendText("����� 1 �ѵȴ�" + wait[0] + "\n����� 2 �ѵȴ�" + wait[1] + "\n����� 3 �ѵȴ�" + wait[2] + "\n");
            disk[0] = disk1.Text;disk[1] = disk2.Text;disk[2] = disk3.Text;//��ȡ��ǰ�����洢��ҳ����Ϣ
            //��ȱҳ��������У�
            for (int i = 0; i < 3; i++)
            {
                if (disk[i] == "")
                {
                    emptyflag = 1;  //����������Ϊ��
                    if(emptydisk == -1)
                    {
                        emptydisk = i;  //�����һ��Ϊ�յ��������
                    }
                }
                if (Convert.ToString(pagelist[vpoint]) == disk[i])
                {
                    lackflag = 0;   //��ʾ��ȱҳ
                    wait[i] = 0;   //���������ȴ�ʱ��(����ĩβ��Ҫͳһ��һ)
                    richTextBox1.AppendText("����ҳ��" + pagelist[vpoint] + "�������" + (i+1) +"�ĵȴ�ʱ������" + "\n");
                    vpoint++;   //��������ָ��
                    break;
                }
            }
            //ȱҳ���
            if (emptyflag == 1 && lackflag == 1)    //���������Ϊ��ʱ
            {
                disk[emptydisk] = Convert.ToString(pagelist[vpoint]); //��ҳ����Ŵ���������
                disk1.Text = disk[0];disk2.Text = disk[1];disk3.Text = disk[2];//����������Ϣ����
                richTextBox1.AppendText("�����" + (emptydisk+1) + "���ڷ���ҳ��" + disk[emptydisk] + "\n");  //���ı�����ʾ�������
                vpoint++;    //��������ָ��
            }
            else if (emptyflag == 0 && lackflag == 1)
            {
                //��ȡ�ȴ���������
                int max = 0;
                for (int i = 0; i < wait.Length; i++)
                {

                    if (wait[i] > wait[max])
                    {
                        max = i;
                    }
                }
                //�û�������δ����ҳ��
                string tempdisk = disk[max];
                disk[max] = Convert.ToString(pagelist[vpoint]);
                richTextBox1.AppendText("�����" + (max+1) + "��ҳ��" + tempdisk + "�û�Ϊҳ��" + disk[max] + "\n");
                disk1.Text = disk[0];disk2.Text = disk[1];disk3.Text = disk[2];//����������Ϣ����
                lackflag = 1;   //��ʾȱҳ
                wait[max] = 0; //���������ȴ�ʱ��
                vpoint++;   //��������ָ��
            }
            //���������Ѵ�ҳ�棩�����ȴ�ʱ��
            for (int i = 0; i < 3; i++)
            {
                if (disk[i] != "")
                {
                    wait[i]++;
                }
            }  
            if (lackflag == 1) 
            {
                lackcounts++;   //ȱҳ������1
            }
            lackflag = 1;   //����ȱҳ��־��Ĭ��ȱҳ�� 
        }
        private void start_Click(object sender, EventArgs e)
        {
            lackcounts = 0; //��ʼ��ȱҳ����
            vpoint = 0; //��ʼ��ָ��
            wait[0] = 0; wait[1] = 0; wait[2] = 0;  //��ʼ�������ȴ�ʱ��
            disk1.Text = "";disk2.Text = "";disk3.Text = "";    //��ʼ�������չʾ�����
            if (algorithm == "None")
            {
                MessageBox.Show("��ѡ���㷨");
            }else timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();   //���س������ģ��
            SystemStartForm SystemStartForm = new SystemStartForm();  //�½���ʼ����
            SystemStartForm.ShowDialog();   //�򿪿�ʼ����
        }
    }
}