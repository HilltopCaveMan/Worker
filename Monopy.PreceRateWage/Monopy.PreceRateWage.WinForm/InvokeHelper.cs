/*
 * HH,2017-6-22
 * Net2.0
 * For Winform
*/

using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Monopy.PreceRateWage.WinForm
{
    public enum InvokeType
    {
        Invoke, BeginInvoke
    }

    public class InvokeData : EventArgs
    {
        public object Sender { get; set; }
        public string Text { get; set; }
        public bool IsTxtAdd { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public bool Visible { get; set; }
        public Color SymbolColor { get; set; }
        public string Symbol { get; set; }
        public bool Enable { get; set; }
        public InvokeType InvokeOrBeginInvoke { get; set; }

        public InvokeData(object sender)
        {
            Sender = sender;
            IsTxtAdd = false;
            Visible = true;
            Enable = true;
            BackColor = Color.Transparent;
            ForeColor = SystemColors.ControlText;
            InvokeOrBeginInvoke = InvokeType.BeginInvoke;
        }

        /// <summary>
        /// 更新控件的数据构造函数
        /// </summary>
        /// <param name="sender">窗体（一般用this），如果更新单个，可以用控件</param>
        /// <param name="updateType">更新方式，Invoke:等待更新后线程再执行；BeginInvoke:不等待</param>
        public InvokeData(object sender, InvokeType updateType)
            : this(sender)
        {
            InvokeOrBeginInvoke = updateType;
        }
    }

    /// <summary>
    /// 其他线程访问UI线程控件
    /// </summary>
    public class InvokeHelper
    {
        /// <summary>
        /// 因为基于2.0，所以自己定义了委托并按照微软2.0的建议写的委托……
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SetInvokeEventHanlder(object sender, InvokeData e);

        /// <summary>
        /// 非UI线程更新单个控件
        /// 目前项目只用到Control和ToolStripItem
        /// 目前更新数据：文本，颜色，可视
        /// 不满足时再扩充。
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="e">更新数据</param>
        public static void SetInvoke(object sender, InvokeData e)
        {
            Control windows = e.Sender as Control;
            if (windows.InvokeRequired)
            {
                if (e.InvokeOrBeginInvoke == InvokeType.BeginInvoke)
                {
                    windows.BeginInvoke(new SetInvokeEventHanlder(SetInvoke), sender, e);
                }
                if (e.InvokeOrBeginInvoke == InvokeType.Invoke)
                {
                    windows.Invoke(new SetInvokeEventHanlder(SetInvoke), sender, e);
                }
                return;
            }
            else
            {
                if (sender is LabelItem labelItem)
                {
                    if (e.IsTxtAdd)
                    {
                        labelItem.Text += e.Text;
                    }
                    else
                    {
                        labelItem.Text = e.Text;
                    }
                    labelItem.BackColor = e.BackColor;
                    labelItem.ForeColor = e.ForeColor;
                    labelItem.Visible = e.Visible;
                    labelItem.SymbolColor = e.SymbolColor;
                    labelItem.Symbol = e.Symbol;
                    labelItem.Enabled = e.Enable;
                    return;
                }
                if (sender is LabelX labelX)
                {
                    if (e.IsTxtAdd)
                    {
                        labelX.Text += e.Text;
                    }
                    else
                    {
                        labelX.Text = e.Text;
                    }
                    labelX.BackColor = e.BackColor;
                    labelX.ForeColor = e.ForeColor;
                    labelX.Visible = e.Visible;
                    labelX.SymbolColor = e.SymbolColor;
                    labelX.Symbol = e.Symbol;
                    labelX.Enabled = e.Enable;
                }
                if (sender is Control control)
                {
                    if (e.IsTxtAdd)
                    {
                        control.Text += e.Text;
                    }
                    else
                    {
                        control.Text = e.Text;
                    }
                    control.BackColor = e.BackColor;
                    control.ForeColor = e.ForeColor;
                    control.Visible = e.Visible;
                    control.Enabled = e.Enable;
                    return;
                }
                if (sender is ToolStripItem toolStripItem)
                {
                    if (e.IsTxtAdd)
                    {
                        toolStripItem.Text += e.Text;
                    }
                    else
                    {
                        toolStripItem.Text = e.Text;
                    }
                    toolStripItem.BackColor = e.BackColor;
                    toolStripItem.ForeColor = e.ForeColor;
                    toolStripItem.Visible = e.Visible;
                    toolStripItem.Enabled = e.Enable;
                    return;
                }
            }
        }

        /// <summary>
        /// 非UI线程更新多个控件
        /// </summary>
        /// <param name="controlsAndInvokeDatas">控件，更新数据</param>
        public static void SetInvoke(Dictionary<object, InvokeData> controlsAndInvokeDatas)
        {
            foreach (var item in controlsAndInvokeDatas.Keys)
            {
                SetInvoke(item, controlsAndInvokeDatas[item]);
            }
        }
    }
}