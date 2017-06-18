"""Add Bookmarks

Revision ID: 0172990b211a
Revises: 4c3b8b749a9c
Create Date: 2017-06-18 19:22:30.323613

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.dialects.postgresql import ARRAY

# revision identifiers, used by Alembic.
revision = '0172990b211a'
down_revision = '4c3b8b749a9c'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'bookmarks',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('link', sa.String(2048), nullable=False),
        sa.Column('description', sa.Text(), nullable=True),
        sa.Column('tags', ARRAY(sa.String(256)), default=[]),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('modified_at', sa.DateTime(timezone=True), nullable=False),
    )


def downgrade():
    op.drop_table('bookmarks')
