using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Employees.Administration.ViewModels;
using Employees.DAL;
using Employees.DAL.Entities;
using Employees.DAL.Migrations;
using Employees.DAL.Repositories;
using Employees.Personally.ViewModels;
using Employees.Shared.Constants;
using Employees.Shared.Models;
using Employees.Shared.Permission;
using Employees.ViewModels;

namespace Employees
{
    internal class EmployeeBootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;


        public EmployeeBootstrapper()
        {
            Start();
        }

        protected override void Configure()
        {
            //DeepBlue - DXStyle - LightGray - MetropolisDark - MetropolisLight - Office2013 - Seven - VS2010 - TouchlineDark
            ThemeManager.ApplicationThemeName = "DXStyle";

            InstallDevExpressConventions();

            ConfigureSimpleContainer();

            ConfigureAutoMapper();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShellViewModel>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            yield return Assembly.GetAssembly(typeof (IShellViewModel));
            yield return Assembly.GetAssembly(typeof (IEmployeeInfoViewModel));
            yield return Assembly.GetAssembly(typeof (IAdministrationWorkspaceViewModel));
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }


        private static void InstallDevExpressConventions()
        {
            ConventionManager.AddElementConvention<BaseEdit>(BaseEdit.EditValueProperty, "EditValue", "EditValueChanged");

            //ConventionManager.AddElementConvention<DataControlBase>(DataControlBase.ItemsSourceProperty, "DataContext", "Loaded");
            //ConventionManager.AddElementConvention<PivotGridControl>(PivotGridControl.DataSourceProperty, "DataContext", "Loaded");
            //ConventionManager.AddElementConvention<DataLayoutControl>(DataLayoutControl.CurrentItemProperty, "DataContext", "Loaded");

            //ConventionManager.AddElementConvention<LookUpEditBase>(LookUpEditBase.ItemsSourceProperty, "SelectedItem", "SelectedIndexChanged").ApplyBinding =
            //    delegate(Type viewModelType, string path, PropertyInfo property, FrameworkElement element, ElementConvention convention)
            //        {
            //            DependencyProperty dependencyProperty = convention.GetBindableProperty(element);
            //            if (!ConventionManager.SetBindingWithoutBindingOrValueOverwrite(viewModelType, path, property, element, convention, dependencyProperty))
            //            {
            //                return false;
            //            }
            //            ConventionManager.ConfigureSelectedItem(element, LookUpEditBase.SelectedItemProperty, viewModelType, path);
            //            return true;
            //        };
            //ConventionManager.AddElementConvention<DocumentGroup>(LayoutGroup.ItemsSourceProperty, "ItemsSource", "SelectedItemChanged").ApplyBinding =
            //    delegate(Type viewModelType, string path, PropertyInfo property, FrameworkElement element, ElementConvention convention)
            //        {
            //            DependencyProperty dependencyProperty = convention.GetBindableProperty(element);
            //            if (!ConventionManager.SetBindingWithoutBindingOverwrite(viewModelType, path, property, element, convention, dependencyProperty))
            //            {
            //                return false;
            //            }
            //            DocumentGroup documentGroup = (DocumentGroup) element;
            //            if (documentGroup.ItemContentTemplate == null && documentGroup.ItemContentTemplateSelector == null && property.PropertyType.IsGenericType)
            //            {
            //                Type type = property.PropertyType.GetGenericArguments().First<Type>();
            //                if (!type.IsValueType && !typeof(string).IsAssignableFrom(type))
            //                {
            //                    documentGroup.ItemContentTemplate = ConventionManager.DefaultItemTemplate;
            //                    if (documentGroup.ItemStyle == null)
            //                    {
            //                        Style style = new Style(typeof(DocumentPanel));
            //                        style.Setters.Add(new Setter(BaseLayoutItem.CloseCommandProperty, new Binding("CloseCommand")));
            //                        documentGroup.ItemStyle = style;
            //                    }
            //                }
            //            }
            //            ConventionManager.ConfigureSelectedItem(element, LayoutGroup.SelectedTabIndexProperty, viewModelType, path);
            //            ConventionManager.ApplyHeaderTemplate(documentGroup, LayoutGroup.ItemCaptionTemplateProperty, LayoutGroup.ItemCaptionTemplateSelectorProperty, viewModelType);
            //            return true;
            //        };
        }

        private void ConfigureSimpleContainer()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            _container.PerRequest<EmployeeContext, EmployeeContext>();
            _container.PerRequest<IEmployeeUnitOfWork, EmployeeUnitOfWork>();
            _container.PerRequest<GenericRepository<EmployeeEntity>, GenericRepository<EmployeeEntity>>();
            _container.PerRequest<UserGroupRepository, UserGroupRepository>();
            _container.PerRequest<UserRepository, UserRepository>();
            _container.PerRequest<PermissionKeyRepository, PermissionKeyRepository>();
            _container.PerRequest<UserGroupPermissionRepository, UserGroupPermissionRepository>();

            //_container.PerRequest<DialogViewModel, DialogViewModel>();
            //_container.AllTypesOf<IScreen>(Assembly.GetAssembly(typeof (EmployeeInfoViewModel)));

            _container.PerRequest<IShellViewModel, ShellViewModel>();
            _container.PerRequest<ILoginViewModel, LoginViewModel>();
            
            _container.PerRequest<IEmployeeInfoViewModel, EmployeeInfoViewModel>();

            _container.PerRequest<IAdministrationWorkspaceViewModel, AdministrationWorkspaceViewModel>();
            _container.PerRequest<IUserViewModel, UserViewModel>();
            _container.PerRequest<IUserGroupWorkspaceViewModel, UserGroupWorkspaceViewModel>();
            _container.PerRequest<IUserGroupViewModel, UserGroupViewModel>();
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<EmployeeEntity, Employee>()
                .ForMember(d => d.IsNotifying, m => m.Ignore())
                .ForMember(d => d.State, m => m.UseValue(ModelStates.Unchanged));
            Mapper.CreateMap<Employee, EmployeeEntity>()
                .ForMember(d => d.CreateUserId, m => m.UseValue<long>(1));

            Mapper.CreateMap<UserGroupEntity, UserGroup>()
                .ForMember(d => d.IsNotifying, m => m.Ignore())
                .ForMember(d => d.State, m => m.UseValue(ModelStates.Unchanged));
            Mapper.CreateMap<UserGroup, UserGroupEntity>()
                .ForMember(d => d.Users, m => m.Ignore())
                .ForMember(d => d.UserGroupPermissions, m => m.Ignore());

            Mapper.CreateMap<UserEntity, User>()
                .ForMember(d => d.IsNotifying, m => m.Ignore())
                .ForMember(d => d.Password, m => m.MapFrom(ue => Encryption.Decrypt(ue.Password)))
                .ForMember(d => d.State, m => m.UseValue(ModelStates.Unchanged));
            Mapper.CreateMap<User, UserEntity>()
                .ForMember(d => d.Password, m => m.MapFrom(u => Encryption.Encrypt(u.Password)));

            Mapper.CreateMap<PermissionKeyEntity, PermissionKey>()
                .ForMember(d => d.IsNotifying, m => m.Ignore())
                .ForMember(d => d.State, m => m.UseValue(ModelStates.Unchanged));
            Mapper.CreateMap<PermissionKey, PermissionKeyEntity>();

            Mapper.CreateMap<UserGroupPermissionEntity, UserGroupPermission>()
                .ForMember(d => d.IsNotifying, m => m.Ignore())
                .ForMember(d => d.State, m => m.UseValue(ModelStates.Unchanged))
                .ForMember(d => d.TreeId, m => m.Ignore())
                .ForMember(d => d.TreeParentId, m => m.Ignore())
                .ForMember(d => d.UserGroup, m => m.MapFrom(s => s.UserGroupEntity))
                .ForMember(d => d.PermissionKey, m => m.MapFrom(s => s.PermissionKeyEntity));
            Mapper.CreateMap<UserGroupPermission, UserGroupPermissionEntity>()
                .ForMember(d => d.UserGroupEntity, m => m.MapFrom(s => s.UserGroup))
                .ForMember(d => d.PermissionKeyEntity, m => m.MapFrom(s => s.PermissionKey));


            Mapper.AssertConfigurationIsValid();
        }
    }
}