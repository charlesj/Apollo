"""Add Notes table

Revision ID: d1cd159b4f82
Revises: 6620d1c95ab9
Create Date: 2017-08-30 17:58:38.136090

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'd1cd159b4f82'
down_revision = '6620d1c95ab9'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'notes',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('name', sa.String(256), nullable=False),
        sa.Column('body', sa.Text(), nullable=True),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('modified_at', sa.DateTime(timezone=True), nullable=False)
    )

def downgrade():
    op.drop_table('notes')
