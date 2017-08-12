"""AddFeeds

Revision ID: 6620d1c95ab9
Revises: bdce5a60ee7d
Create Date: 2017-08-12 14:52:19.067761

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '6620d1c95ab9'
down_revision = 'bdce5a60ee7d'
branch_labels = None
depends_on = None

feed_table_name = 'feeds'
feed_items_table_name = 'feed_items'

def upgrade():
    op.create_table(
        feed_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('url', sa.String(2048), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('last_updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('last_attempted', sa.DateTime(timezone=True), nullable=False)
    )

    op.create_table(
        feed_items_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('feed_id', sa.Integer, nullable=False),
        sa.Column('title', sa.String(256), nullable=False),
        sa.Column('url', sa.String(2048), nullable=False),
        sa.Column('body', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('published_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('read_at', sa.DateTime(timezone=True), nullable=True)
    )


def downgrade():
    op.drop_table(feed_items_table_name)
    op.drop_table(feed_table_name)
