"""Add more tracking to login sessions

Revision ID: bf021960744f
Revises: 577f2365a104
Create Date: 2017-05-27 16:51:25.913243

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'bf021960744f'
down_revision = '577f2365a104'
branch_labels = None
depends_on = None

table_name='login_sessions'

def upgrade():
    op.add_column(
        table_name,
        sa.Column('ip_address', sa.String(256), default='')
    )

    op.add_column(
        table_name,
        sa.Column('user_agent', sa.String(256), default='')
    )

    op.add_column(
        table_name,
        sa.Column('revoked', sa.Boolean(), default=False)
    )


def downgrade():
    op.drop_column(table_name, 'ip_address')
    op.drop_column(table_name, 'user_agent')
    op.drop_column(table_name, 'revoked')
