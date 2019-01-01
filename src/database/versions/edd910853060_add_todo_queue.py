"""Add Todo Queue

Revision ID: edd910853060
Revises: 36bda0ac6dec
Create Date: 2017-07-29 18:33:25.286207

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'edd910853060'
down_revision = '36bda0ac6dec'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'todo_queue_items',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('link', sa.String(2048), nullable=True),
        sa.Column('description', sa.Text(), nullable=True),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('completed_at', sa.DateTime(timezone=True), nullable=True)
    )

def downgrade():
    op.drop_table('todo_queue_items')
