"""Add Checklists

Revision ID: cdb2997e1c4b
Revises: 9422e46956fb
Create Date: 2017-11-24 18:56:32.227650

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'cdb2997e1c4b'
down_revision = '9422e46956fb'
branch_labels = None
depends_on = None


checklists_table_name = 'checklists'
checklist_items_table_name = 'checklist_items'
checklist_completion_table_name = 'checklist_completions'
checklist_completion_items_table_name = 'checklist_completion_items'

def upgrade():
    op.create_table(
        checklists_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('type', sa.String(32), nullable=False),
        sa.Column('description', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )

    op.create_table(
        checklist_items_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('checklist_id', sa.Integer, nullable=False),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('type', sa.String(32), nullable=False),
        sa.Column('description', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )

    op.create_table(
        checklist_completion_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('checklist_id', sa.Integer, nullable=False),
        sa.Column('notes', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )

    op.create_table(
        checklist_completion_items_table_name,
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('checklist_completion_id', sa.Integer, nullable=False),
        sa.Column('checklist_item_id', sa.Integer, nullable=False),
        sa.Column('completed', sa.Integer, nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('updated_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('deleted_at', sa.DateTime(timezone=True), nullable=True)
    )

def downgrade():
    op.drop_table(checklists_table_name)
    op.drop_table(checklist_items_table_name)
    op.drop_table(checklist_completion_table_name)
    op.drop_table(checklist_completion_items_table_name)
