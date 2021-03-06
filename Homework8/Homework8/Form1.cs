﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework11
{
    public partial class frmMain : Form
    {
        OrderService service = new OrderService();
        public string queryKeyWord { get; set; }
        public static List<Goods> shopgoods = new List<Goods> { new Goods("fish", 10),
                                                  new Goods("gun", 100),
                                                  new Goods("wine", 15) };
        public frmMain()
        {
            InitializeComponent();
            /*   OrderItem newItem1 = new OrderItem(shopgoods[0], 1);
               Customer person1 = new Customer("lz");
               Order order1 = new Order(1, person1);
               order1.addOrderItem(newItem1);
               service.addOrder(order1);

               OrderItem newItem2 = new OrderItem(shopgoods[1], 2);
               Customer person2 = new Customer("5z");
               Order order2 = new Order(2, person2);
               order2.addOrderItem(newItem2);
               service.addOrder(order2)；*/
            orderBindingSource.DataSource = OrderService.GetAllOrders();
            txtQueryName.DataBindings.Add("Text", this, "queryKeyWord");

        }


        private void dgvOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //orderItemBindingSource.DataSource = service.orderList.Find(x => x.Id == int.Parse(dgvOrder.Rows[e.RowIndex].Cells[0].Value.ToString())).orderItemList;
            Order COrder = orderBindingSource.Current as Order;
            orderItemBindingSource.DataSource = COrder.orderItemList;
        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            switch (cboQueryType.SelectedIndex)
            {
                case 0:
                    btnQuery.Click += btnQuery_Click_queryByID;
                    break;
                case 1:
                    btnQuery.Click += btnQuery_Click_queryByCustomer;
                    break;
                case 2:
                    btnQuery.Click += btnQuery_Click_queryByGoodType;
                    break;
                default:
                    btnQuery.Click += btnQuery_Click_queryByID;
                    break;
            }

        }
        private void btnQuery_Click_queryByID(object sender, EventArgs e)
        {
            if (queryKeyWord == null || queryKeyWord == "")
                orderBindingSource.DataSource = OrderService.GetAllOrders();
            else
            {
                int temp;
                int.TryParse(queryKeyWord, out temp);
                orderBindingSource.DataSource=OrderService.inquiryOrder(temp);
                //orderBindingSource.DataSource = service.orderList.Where(x => x.Id.ToString() == txtQueryName.Text);               
            }
        }
        private void btnQuery_Click_queryByCustomer(object sender, EventArgs e)
        {
            if (queryKeyWord == null || queryKeyWord == "")
                orderBindingSource.DataSource = OrderService.GetAllOrders();
            else
                orderBindingSource.DataSource = OrderService.inquiryOrder(queryKeyWord);
              //  orderBindingSource.DataSource = service.orderList.Where(x => x.Cname == txtQueryName.Text);
        }
        private void btnQuery_Click_queryByGoodType(object sender, EventArgs e)
        {
            if (queryKeyWord == null || queryKeyWord == "")
                orderBindingSource.DataSource = OrderService.GetAllOrders();
            else
                orderBindingSource.DataSource = OrderService.inquiryOrderByType((shopgoods.Find(x => x.Type.ToString() == queryKeyWord).Type));
               /* orderBindingSource.DataSource = service.orderList.Where(x => x.orderItemList.Exists(y => y.thisgoods.ToString() == txtQueryName.Text))
                                 .OrderByDescending(x => x.Totalprice);*/
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmAddOrder form2 = new frmAddOrder(new Order(),true);
            if (form2.ShowDialog() == DialogResult.OK)
            {
                //form2.CurrentOrder.CustomerId = form2.CurrentOrder.Customer.ID;
                try
                {
                    OrderService.addOrder(form2.CurrentOrder);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                                  
                orderBindingSource.DataSource = OrderService.GetAllOrders();
                orderBindingSource.ResetBindings(false);

            }
        }

        //用于将orderlist传递到form2窗口
        private void orderBindingSource_DataSourceChanged(object sender, EventArgs e)
        {
            frmMain_parameter.orderlist = OrderService.GetAllOrders();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            Order ClickedOrder = orderBindingSource.Current as Order;
            OrderService.deleteOrder(ClickedOrder.OrderId);
            orderBindingSource.DataSource = OrderService.GetAllOrders();
            orderBindingSource.ResetBindings(false);
            orderItemBindingSource.DataSource = null;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            frmAddOrder form2 = new frmAddOrder(dgvOrder.CurrentRow.DataBoundItem as Order , false);
            if (form2.ShowDialog() == DialogResult.OK)
            {
                OrderService.modifyOrder(form2.CurrentOrder);
                //service.orderList.Find(x => x.id == form2.CurrentOrder.Id).updateTotalprice();
                orderBindingSource.DataSource = OrderService.GetAllOrders();
                orderBindingSource.ResetBindings(false);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                String fileName = saveFileDialog1.FileName;
                OrderService.Export(fileName);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                String fileName = openFileDialog1.FileName;
                OrderService.Import(fileName);
                orderBindingSource.DataSource = OrderService.GetAllOrders();
                orderBindingSource.ResetBindings(false);
            }
        }
    }
}



