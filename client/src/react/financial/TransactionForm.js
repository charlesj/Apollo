import React from 'react'
import PropTypes from 'prop-types'
import { connect, } from 'react-redux'
import { Field, reduxForm, } from 'redux-form'
import Flexbox from 'flexbox-react'
import {
  CancelButton,
  SaveButton,
  Container,
  RemoveButton,
} from '../_controls'
import './transactionForm.css'

function TransactionForm(props) {
  const { handleSubmit, onCancel, onDelete, } = props
  return (
    <Container className="transactionFormContainer" width={400}>
      <form onSubmit={handleSubmit}>
        <Flexbox flexDirection='column'>
          <Field
            name="name"
            component="input"
            type="text"
            placeholder="Name"
          />
          <Field
            name="occurred_at"
            component="input"
            type="datetime-local"
            placeholder="Transaction Time"
          />
          <Field
            name="amount"
            component="input"
            type="text"
            placeholder="Amount"
          />
          <Field
            name="tagsDisplay"
            component="input"
            type="text"
            placeholder="Tags"
          />
          <Field
            name="notes"
            component="textarea"
            type="text"
            placeholder="Notes"
          />
          <Flexbox justifyContent="flex-end" flexGrow={1}>
            <CancelButton type="button" onClick={onCancel} />
            {onDelete && <RemoveButton onClick={onDelete} />}
            <SaveButton type="submit" primary />
          </Flexbox>
        </Flexbox>
      </form>
    </Container>
  )
}

TransactionForm.propTypes = {
  transaction: PropTypes.object.isRequired,
  onSubmit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onDelete: PropTypes.func,
  handleSubmit: PropTypes.func.isRequired,
}

TransactionForm = reduxForm({ // eslint-disable-line no-func-assign
  form: 'FinancialTransactionForm',
  enableReinitialize: true,
})(TransactionForm)

function mapStateToProps(state, props) {
  const { transaction, } = props

  return {
    initialValues: transaction,
  }
}

export default connect(mapStateToProps)(TransactionForm)