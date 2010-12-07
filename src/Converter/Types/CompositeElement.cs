using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Cfg.MappingSchema;
using NHibernateHbmToFluent.Converter.Extensions;

namespace NHibernateHbmToFluent.Converter.Types
{
    public class CompositeElement
    {
        private readonly CodeFileBuilder _builder;

		public CompositeElement(CodeFileBuilder builder)
		{
			_builder = builder;
		}

		public void Start(string prefix, MappedPropertyInfo item)
		{
			CodeFileBuilder componentBuilder = new CodeFileBuilder();
			componentBuilder.Indent(5);
			const string subPrefix = "y.";
			HbmCompositeElement component = item.HbmObject<HbmCompositeElement>();
			componentBuilder.AddLine("");

			var componentBodyBuilder = new ClassMapBody(componentBuilder);
			foreach (var componentPart in component.Items)
			{
				componentBodyBuilder.Add(subPrefix, new MappedPropertyInfo(componentPart, item.FileName));
			}
			_builder.StartMethod(prefix, string.Format("{0}<{1}>(x => x.{2}, y=>", FluentNHibernateNames.CompositeElement, item.ReturnType, item.Name));
			_builder.AddLine("{");
			_builder.AddLine(componentBuilder.ToString());
			_builder.AddLine("})");
			/*if (component. )
			{
				_builder.AddLine(string.Format(".{0}()", FluentNHibernateNames.Insert));
			}
			if (component.update)
			{
				_builder.AddLine(string.Format(".{0}()", FluentNHibernateNames.Update));
			}*/
		}

		public static class FluentNHibernateNames
		{
			public static string CompositeElement // is using same name as Component in fluent
			{
				get { return ReflectionUtility.GetMethodName((FakeMap f) => f.Component(x => x, null)); }
			}

			public static string Insert
			{
				get { return ReflectionUtility.GetMethodName((FakeMap f) => f.Component(x => x, null).Insert()); }
			}

			public static string Update
			{
				get { return ReflectionUtility.GetMethodName((FakeMap f) => f.Component(x => x, null).Update()); }
			}
		}
	}
}

