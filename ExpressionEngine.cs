using System;
using System.Linq.Expressions;
namespace Core.Engines
{
	public class ExpressionEngine<TEntity> where TEntity : class
	{
		private readonly ParameterExpression _parameter;

		public ExpressionEngine()
		{
			_parameter = Expression.Parameter(typeof(TEntity), "_");
		}

		public Expression<Func<TEntity, bool>> CreateContains<TPropertyType>(string propertyName, TPropertyType propertyValue)
		{
			var member = Expression.Property(_parameter, propertyName);
			var constant = Expression.Constant(propertyValue);

			var propertyType = typeof(TEntity).GetProperty(propertyName)?.PropertyType;
			var method = propertyType?.GetMethod("Contains", new[] { typeof(TPropertyType) });

			if (method == null)
				return null;
			var body = Expression.Call(member, method, constant);
			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateEquals<TPropertyType>(string propertyName, TPropertyType propertyValue)
		{
			var member = Expression.Property(_parameter, propertyName);
			var constant = Expression.Constant(propertyValue, typeof(TPropertyType));
			var body = Expression.Equal(member, constant);

			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateNotEquals<TPropertyType>(string propertyName, TPropertyType propertyValue)
		{
			var member = Expression.Property(_parameter, propertyName);
			var constant = Expression.Constant(propertyValue, typeof(TPropertyType));
			var body = Expression.NotEqual(member, constant);

			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateStartWith<TPropertyType>(string propertyName, TPropertyType propertyValue)
		{
			var member = Expression.Property(_parameter, propertyName);
			var constant = Expression.Constant(propertyValue);

			var propertyType = typeof(TEntity).GetProperty(propertyName)?.PropertyType;
			var method = propertyType?.GetMethod("StartsWith", new[] { typeof(TPropertyType) });

			if (method == null)
				return null;
			var body = Expression.Call(member, method, constant);
			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateGreaterThanOrEqual<TPropertyType>(string propertyName, TPropertyType propertyValue)
		{
			var member = Expression.Property(_parameter, propertyName);
			var constant = Expression.Constant(propertyValue, typeof(TPropertyType));
			var body = Expression.GreaterThanOrEqual(member, constant);

			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateLessThanOrEqual<TPropertyType>(string propertyName, TPropertyType propertyValue)
		{
			var member = Expression.Property(_parameter, propertyName);
			var constant = Expression.Constant(propertyValue, typeof(TPropertyType));
			var body = Expression.LessThanOrEqual(member, constant);

			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateAnd(Expression<Func<TEntity, bool>> left, Expression<Func<TEntity, bool>> right)
		{
			var body = Expression.AndAlso(left.Body, right.Body);
			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateOr(Expression<Func<TEntity, bool>> left, Expression<Func<TEntity, bool>> right)
		{
			var body = Expression.OrElse(left.Body, right.Body);
			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateNot(Expression<Func<TEntity, bool>> exp)
		{
			var body = Expression.Not(exp.Body);
			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

		public Expression<Func<TEntity, bool>> CreateMemberContains<TPropertyType>(TPropertyType propertyValue, params string[] properties)
		{
			Expression lastMember = _parameter;
			var constant = Expression.Constant(propertyValue);


			foreach (var property in properties)
			{
				var member = Expression.Property(lastMember, property);
				lastMember = member;
			}

			var method = lastMember.Type.GetMethod("Contains", new[] { typeof(TPropertyType) });
			if (method == null)
				return null;
			var body = Expression.Call(lastMember, method, constant);
			return Expression.Lambda<Func<TEntity, bool>>(body, _parameter);
		}

	}
}
