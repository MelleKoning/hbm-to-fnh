using NHibernate.Cfg.MappingSchema;
using NHibernateHbmToFluent.Converter.Extensions;
using NHibernateHbmToFluent.Converter.Methods.Join;

namespace NHibernateHbmToFluent.Converter.Types
{
	public class Set : IMapStart
	{
		private readonly CodeFileBuilder _builder;
		private readonly Where _where;
		private readonly OrderBy _orderBy;
		private readonly Cascade _cascade;
		private readonly Inverse _inverse;
		private readonly Table _table;
		private readonly KeyColumn _keyColumn;
		private readonly LazyLoad _lazyLoad;

		public Set(CodeFileBuilder builder)
		{
			_builder = builder;
			_where = new Where(builder);
			_orderBy = new OrderBy(builder);
			_cascade = new Cascade(builder);
			_inverse = new Inverse(builder);
			_table = new Table(builder);
			_keyColumn = new KeyColumn(builder);
			_lazyLoad = new LazyLoad(builder);
		}

		public void Start(string prefix, MappedPropertyInfo item)
		{
			HbmSet set = item.HbmObject<HbmSet>();
			MappedPropertyInfo childItem = new MappedPropertyInfo(set.Item, item.FileName);
			PropertyMappingType subType = childItem.Type;
			if (subType == PropertyMappingType.ManyToMany)
			{
				_builder.StartMethod(prefix, "HasManyToMany<" + item.ReturnType + ">(x => x." + item.Name + ")");
				_builder.AddLine(".ChildKeyColumn(\"" + childItem.ColumnName + "\")");
			}
			else if (subType == PropertyMappingType.OneToMany)
			{
				_builder.StartMethod(prefix, "HasMany<" + item.ReturnType + ">(x => x." + item.Name + ")");
			}
			else
			{
				_builder.StartMethod(prefix, "set?(x => x" + item.Name + ")");
			}
			_builder.AddLine(".AsSet()");
			_keyColumn.Add(set.inverse, item.ColumnName, subType);
			_lazyLoad.Add(set.lazySpecified, set.lazy);
			_table.Add(set.table);
			_inverse.Add(set.inverse);
			_cascade.Add(set.cascade);
			_orderBy.Add(set.orderby);
			_where.Add(set.where);
		}
	}
}