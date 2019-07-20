"""Create Metrics Table

Revision ID: c7f6c1fe4bf9
Revises: bf021960744f
Create Date: 2017-06-16 00:36:40.863461

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'c7f6c1fe4bf9'
down_revision = 'bf021960744f'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'metrics',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('category', sa.String(100), nullable=False),
        sa.Column('name', sa.String(100), nullable=False),
        sa.Column('value', sa.Float(), nullable=False),
        sa.Column('created_at', sa.DateTime(), nullable=False)
    )

def downgrade():
    op.drop_table('metrics')
