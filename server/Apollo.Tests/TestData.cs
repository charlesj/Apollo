using System;
using Apollo.Data;

namespace Apollo.Tests
{
    public static class TestData
    {
        public static class TodoItems
        {
            public static TodoItem TodoItem(
                int id=1,
                string title="title",
                DateTime? created_at = null,
                DateTime? completed_at = null)
            {
                if (created_at == null)
                {
                    created_at = DateTime.UtcNow;
                }

                return new TodoItem
                {
                    id= id,
                    title = title,
                    completed_at = completed_at,
                    created_at = created_at.Value
                };
            }
        }
    }
}
