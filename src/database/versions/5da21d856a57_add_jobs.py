"""Add Jobs

Revision ID: 5da21d856a57
Revises: edd910853060
Create Date: 2017-07-30 16:43:35.839411

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '5da21d856a57'
down_revision = 'edd910853060'
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'jobs',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('command_name', sa.String(100), nullable=False),
        sa.Column('parameters', sa.Text(), nullable=False),
        sa.Column('schedule', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('last_executed_at', sa.DateTime(timezone=True), nullable=True),
        sa.Column('expired_at', sa.DateTime(timezone=True), nullable=True),
    )

    op.create_table(
        'job_history',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('job_id', sa.Integer, nullable=False),
        sa.Column('execution_id', sa.String(256), nullable=False),
        sa.Column('executed_at', sa.DateTime(timezone=True), nullable=False),
        sa.Column('execution_ended', sa.DateTime(timezone=True), nullable=True),
        sa.Column('results', sa.Text(), nullable=True),
        sa.Column('result_type', sa.String(100), nullable=True),
    )

def downgrade():
    op.drop_table('jobs')
    op.drop_table('job_history')
