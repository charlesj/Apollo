"""Add Boards

Revision ID: 50e85a64aa30
Revises: d1cd159b4f82
Create Date: 2017-11-11 20:15:43.683292

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '50e85a64aa30'
down_revision = 'd1cd159b4f82'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'boards',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('list_order', sa.Integer, nullable=True),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False)
    )
    op.create_table(
        'board_items',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('board_id', sa.Integer, nullable=False),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('link', sa.String(2048), nullable=True),
        sa.Column('description', sa.Text(), nullable=True),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('completed_at', sa.DateTime(timezone=True), nullable=True)
    )

def downgrade():
    op.drop_table('boards')
    op.drop_table('board_items')
