﻿using System;
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
using SolrEngine;

namespace StudioWPF.DataAccess
{
    /// <summary>
    /// Interaction logic for SolrSearch.xaml
    /// </summary>
    public partial class SolrSearch : Page
    {
        private readonly ISolrManager _manager;

        public SolrSearch(ISolrManager manager)
        {
            _manager = manager;
            InitializeComponent();
        }
    }
}