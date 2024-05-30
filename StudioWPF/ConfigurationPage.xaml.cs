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
using SqlData.Context;

namespace StudioWPF
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        private readonly AppDbContext _context;

        public ConfigurationPage(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
            SetParamsValue();
        }

        private void SetParamsValue()
        {
            txtSolrUrl.Text = _context.AppParameters.FirstOrDefault(x => x.Name == "SolrUrl")?.Value;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var param = _context.AppParameters.FirstOrDefault(x => x.Name == "SolrUrl");
            if (param != null)
            {
                param.Value = txtSolrUrl.Text;
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("There is no SolrUrl param on database");
            }
           
        }
    }
}
