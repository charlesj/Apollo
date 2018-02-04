"""add timeline

Revision ID: 2cd16ae229a1
Revises: f0f2dc8b6d5e
Create Date: 2018-02-04 17:12:08.093091

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '2cd16ae229a1'
down_revision = 'f0f2dc8b6d5e'
branch_labels = None
depends_on = None

table_name='timeline'

def upgrade():
    op.create_table(
        table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('title', sa.String(1024), nullable=False),
        sa.Column('reference_id', sa.Integer, nullable=True),
        sa.Column('reference_type', sa.String(64), nullable=True),
        sa.Column('event_time', sa.DateTime(timezone=True), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
    )

def downgrade():
    op.drop_table(table_name)
