"""Add Financial Asset Price Table

Revision ID: bdce5a60ee7d
Revises: 5da21d856a57
Create Date: 2017-08-01 20:40:11.691278

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'bdce5a60ee7d'
down_revision = '5da21d856a57'
branch_labels = None
depends_on = None

table_name = 'financial_asset_prices'

def upgrade():
    op.create_table(
        table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('symbol', sa.String(8), nullable=False),
        sa.Column('price', sa.Float(precision=4), nullable=False),
        sa.Column('source', sa.String(8), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('valid_at', sa.DateTime(timezone=True), nullable=False),
    )

def downgrade():
    op.drop_table(table_name)
