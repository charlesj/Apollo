"""Add Tags For JournalEntries

Revision ID: 577f2365a104
Revises: ce592be7e0e9
Create Date: 2017-05-14 19:29:28.586678

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.dialects.postgresql import ARRAY

# revision identifiers, used by Alembic.
revision = '577f2365a104'
down_revision = 'ce592be7e0e9'
branch_labels = None
depends_on = None


def upgrade():
    op.add_column(
        'journal',
        sa.Column('tags', ARRAY(sa.String(256)), default=[])
    )


def downgrade():
    op.drop_column('journal', 'tags')
