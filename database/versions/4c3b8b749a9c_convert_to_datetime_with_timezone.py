"""Convert to datetime with timezone

Revision ID: 4c3b8b749a9c
Revises: c7f6c1fe4bf9
Create Date: 2017-06-17 17:32:23.257908

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy import Column


# revision identifiers, used by Alembic.
revision = '4c3b8b749a9c'
down_revision = 'c7f6c1fe4bf9'
branch_labels = None
depends_on = None


def upgrade():
    op.alter_column('journal', 'created_at', type_=sa.DateTime(timezone=True))
    op.alter_column('user_settings', 'created_at', type_=sa.DateTime(timezone=True))
    op.alter_column('user_settings', 'updated_at', type_=sa.DateTime(timezone=True))
    op.alter_column('login_sessions', 'created_at', type_=sa.DateTime(timezone=True))
    op.alter_column('login_sessions', 'last_seen', type_=sa.DateTime(timezone=True))
    op.alter_column('metrics', 'created_at', type_=sa.DateTime(timezone=True))


def downgrade():
    op.alter_column('journal', 'created_at', type_=sa.DateTime(timezone=False))
    op.alter_column('user_settings', 'created_at', type_=sa.DateTime(timezone=False))
    op.alter_column('user_settings', 'updated_at', type_=sa.DateTime(timezone=False))
    op.alter_column('login_sessions', 'created_at', type_=sa.DateTime(timezone=False))
    op.alter_column('login_sessions', 'last_seen', type_=sa.DateTime(timezone=False))
    op.alter_column('metrics', 'created_at', type_=sa.DateTime(timezone=False))
