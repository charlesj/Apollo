"""Create Login Sessions Table

Revision ID: e5c73fd17a7f
Revises: b738513b07d5
Create Date: 2017-04-30 20:25:00.392898

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'e5c73fd17a7f'
down_revision = 'b738513b07d5'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'login_sessions',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('token', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(), nullable=False),
        sa.Column('last_seen', sa.DateTime(), nullable=False)
    )


def downgrade():
    op.drop_table('login_sessions')
