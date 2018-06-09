"""Add financial accounts and transactions

Revision ID: 0f5e47ad79bf
Revises: 2cd16ae229a1
Create Date: 2018-06-09 18:21:26.287275

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '0f5e47ad79bf'
down_revision = '2cd16ae229a1'
branch_labels = None
depends_on = None


financial_accounts_table_name = 'financial_accounts'
financial_transaction_types_table_name = 'financial_transaction_types'
financial_transactions_table_name = 'financial_transactions'

def upgrade():
    op.create_table(
        financial_accounts_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('type', sa.String(32), nullable=False),
        sa.Column('description', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )

    op.create_table(
        financial_transaction_types_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('description', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )

    op.create_table(
        financial_transactions_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('account_id', sa.Integer, nullable=False),
        sa.Column('transaction_type_id', sa.Integer, nullable=False),
        sa.Column('occurred_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('amount', sa.Float(precision=4), nullable=False),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('notes', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )


def downgrade():
    op.drop_table(financial_accounts_table_name)
    op.drop_table(financial_transaction_types_table_name)
    op.drop_table(financial_transactions_table_name)