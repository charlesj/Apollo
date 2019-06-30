"""Add Todo Items

Revision ID: 36bda0ac6dec
Revises: 0172990b211a
Create Date: 2017-07-04 16:09:18.372866

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '36bda0ac6dec'
down_revision = '0172990b211a'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'todo_items',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('completed_at', sa.DateTime(timezone=True), nullable=True)
    )

def downgrade():
    op.drop_table('todo_items')
