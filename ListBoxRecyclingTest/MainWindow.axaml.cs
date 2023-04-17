using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.VisualTree;

using System;
using System.Collections.Generic;

namespace ListBoxRecyclingTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mChangeListData.Click += ChangeListData_Click;
            mChangeListData2.Click += ChangeListData2_Click;

            mList.DataTemplates.Add(new FuncDataTemplate<RepositoryItemModel>((_, _) =>
                    new RepositoryPanel(), true));

            mList.DataTemplates.Add(new FuncDataTemplate<WorkspaceItemModel>((_, _) =>
                new WorkspacePanel(), true));

            mData1.Add(new RepositoryItemModel("books"));
            mData1.Add(new WorkspaceItemModel("books1"));
            mData1.Add(new RepositoryItemModel("codice"));
            mData1.Add(new WorkspaceItemModel("codice1"));
            mData1.Add(new WorkspaceItemModel("codice2"));
            mData1.Add(new RepositoryItemModel("codice/theme"));
            mData1.Add(new WorkspaceItemModel("codice/theme1"));
            mData1.Add(new RepositoryItemModel("documentation/doc"));
            mData1.Add(new WorkspaceItemModel("documentation/doc-1"));
            mData1.Add(new RepositoryItemModel("cleanstate"));
            mData1.Add(new RepositoryItemModel("spare-shallow"));
            mData1.Add(new RepositoryItemModel("codice/osc"));
            mData1.Add(new RepositoryItemModel("codice/unitymerge"));
            mData1.Add(new RepositoryItemModel("cycletimedashboard"));
            mData1.Add(new RepositoryItemModel("devops"));
            mData1.Add(new RepositoryItemModel("documentation"));
            mData1.Add(new RepositoryItemModel("documentation/plasticdocu"));
            mData1.Add(new RepositoryItemModel("documentation/taskcodumentation"));
            mData1.Add(new RepositoryItemModel("installers"));
            mData1.Add(new RepositoryItemModel("libgit2sharp"));
            mData1.Add(new RepositoryItemModel("licensetools"));
            mData1.Add(new RepositoryItemModel("marketing"));
            mData1.Add(new RepositoryItemModel("nervathirdparty"));

            mData2.Add(new RepositoryItemModel("extensions"));
            mData2.Add(new WorkspaceItemModel("extensions1"));
            mData2.Add(new RepositoryItemModel("miriam-conflict-tests"));
            mData2.Add(new WorkspaceItemModel("miriam-conflict-tests-1"));
            mData2.Add(new WorkspaceItemModel("miriam-conflict-tests-2"));
            mData2.Add(new RepositoryItemModel("viorepo"));
            mData2.Add(new WorkspaceItemModel("viorepo-1"));
            mData2.Add(new WorkspaceItemModel("viorepo-2"));
            mData2.Add(new RepositoryItemModel("2d-URP-Sample-Project"));
            mData2.Add(new RepositoryItemModel("30083"));
            mData2.Add(new RepositoryItemModel("another-migrate"));
            mData2.Add(new RepositoryItemModel("AssetStoreUpdate1"));
            mData2.Add(new RepositoryItemModel("AssetStoreUpdate2"));
            mData2.Add(new RepositoryItemModel("AssetStoreUpdate3"));
            mData2.Add(new RepositoryItemModel("AssetStoreUpdate4"));
            mData2.Add(new RepositoryItemModel("BattleShip"));
            mData2.Add(new RepositoryItemModel("3200_macOS"));
            mData2.Add(new RepositoryItemModel("BL5262"));
            mData2.Add(new RepositoryItemModel("BlenderTest"));
            mData2.Add(new RepositoryItemModel("cr1"));
            mData2.Add(new RepositoryItemModel("extensions12"));
            mData2.Add(new RepositoryItemModel("fantasy"));
            mData2.Add(new RepositoryItemModel("fast"));
            mData2.Add(new RepositoryItemModel("fast/asdh"));
            mData2.Add(new RepositoryItemModel("fast1"));
            mData2.Add(new RepositoryItemModel("fast2"));

            SetData(mData1);

            mList.ContainerClearing += List_ContainerClearing;
        }

        void List_ContainerClearing(object? sender, ContainerClearingEventArgs e)
        {
            if (e.Container is not ListBoxItem container)
                return;

            if (e.Container.DataContext is not ListBoxItemModel model)
                return;

            IDisposable? itemToDispose = null;
            if (model is RepositoryItemModel)
            {
                itemToDispose = container.FindDescendantOfType<RepositoryPanel>();
            }

            if (model is WorkspaceItemModel)
            {
                itemToDispose = container.FindDescendantOfType<WorkspacePanel>();
            }

            System.Diagnostics.Debug.WriteLine("Disposed " + model.Name);
            itemToDispose?.Dispose();
        }

        void ChangeListData_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SetData(mData1);
        }

        void ChangeListData2_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SetData(mData2);
        }

        void SetData(List<ListBoxItemModel> data)
        {
            mList.ItemsSource = data;
        }

        internal abstract class ListBoxItemModel
        {
            internal string Name { get; private set; }

            internal ListBoxItemModel(string name)
            {
                Name = name;
            }
        }

        internal class RepositoryItemModel : ListBoxItemModel
        {
            public RepositoryItemModel(string name) : base(name)
            {
            }

        }
        internal class WorkspaceItemModel : ListBoxItemModel
        {
            public WorkspaceItemModel(string name) : base(name)
            {
            }
        }

        internal class RepositoryPanel : StackPanel, IDisposable
        {
            internal RepositoryPanel()
            {
                mTitleTextBlock = new TextBlock();
                mNameTextBlock = new TextBlock();

                Children.Add(mTitleTextBlock);
                Children.Add(mNameTextBlock);
            }

            protected override void OnDataContextChanged(EventArgs e)
            {
                base.OnDataContextChanged(e);

                if (mIsDisposed)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING!: reusing a container that is already cleared: " + mNameTextBlock.Text);
                    return;
                }

                if (DataContext is not RepositoryItemModel repository)
                {
                    Dispose();
                    return;
                }

                mTitleTextBlock.Text = "REP";
                mNameTextBlock.Text = repository.Name;
            }

            public void Dispose()
            {
                mIsDisposed = true;
            }

            bool mIsDisposed;
            TextBlock mTitleTextBlock;
            TextBlock mNameTextBlock;
        }

        internal class WorkspacePanel : StackPanel, IDisposable
        {
            internal WorkspacePanel()
            {
                mTitleTextBlock = new TextBlock();
                mNameTextBlock = new TextBlock();

                Children.Add(mTitleTextBlock);
                Children.Add(mNameTextBlock);

                Margin = new Avalonia.Thickness(35, 0, 0, 0);
            }

            protected override void OnDataContextChanged(EventArgs e)
            {
                base.OnDataContextChanged(e);

                if (mIsDisposed)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING!: reusing a container that is already cleared: " + mNameTextBlock.Text);
                    return;
                }

                if (DataContext is not WorkspaceItemModel repository)
                {
                    Dispose();
                    return;
                }

                mTitleTextBlock.Text = "WK";
                mNameTextBlock.Text = repository.Name;
            }

            public void Dispose()
            {
                mIsDisposed = true;
            }

            bool mIsDisposed;
            TextBlock mTitleTextBlock;
            TextBlock mNameTextBlock;
        }

        List<ListBoxItemModel> mData1 = new List<ListBoxItemModel>();
        List<ListBoxItemModel> mData2 = new List<ListBoxItemModel>();
    }
}