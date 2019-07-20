"""remove todo and queue

Revision ID: 9422e46956fb
Revises: 50e85a64aa30
Create Date: 2017-11-17 21:01:58.813545

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '9422e46956fb'
down_revision = '50e85a64aa30'
branch_labels = None
depends_on = None


def upgrade():
    op.drop_table('todo_queue_items')
    op.drop_table('todo_items')


def downgrade():
    op.create_table(
        'todo_queue_items',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('link', sa.String(2048), nullable=True),
        sa.Column('description', sa.Text(), nullable=True),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('completed_at', sa.DateTime(timezone=True), nullable=True)
    )

    op.create_table(
        'todo_items',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('completed_at', sa.DateTime(timezone=True), nullable=True)
    )
