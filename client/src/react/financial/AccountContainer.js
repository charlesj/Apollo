import React from 'react'
import PropTypes from 'prop-types'
import { connect, } from 'react-redux'
import Flexbox from 'flexbox-react'
import {
  Container,
  TextButton,
} from '../_controls'
import AccountForm from './AccountForm'
import AccountDisplay from './AccountDisplay'
import { financialSelectors, } from '../../redux/selectors'
import { financialActions, } from '../../redux/actions'
import './accountContainer.css'

function AccountContainer(props) {

  const {
    selectedAccount,
    saveAccount,
    accountMode,
    setAccountMode,
  } = props
  const isEditMode = (accountMode==='edit')

  return (
    <Container width={500}>
      <Flexbox className='accountTitle'>
        <Flexbox flexGrow={1}>{selectedAccount.name}</Flexbox>
        <Flexbox><TextButton onClick={() => setAccountMode('edit')}>edit</TextButton></Flexbox>
      </Flexbox>
      { isEditMode &&
        <AccountForm
          account={selectedAccount}
          onSubmit={saveAccount}
          onCancel={() => setAccountMode('view')}
        />}
      { ( !isEditMode && <AccountDisplay account={selectedAccount} />)}
    </Container>
  )
}


AccountContainer.propTypes = {
  selectedAccount: PropTypes.object.isRequired,
  saveAccount: PropTypes.func.isRequired,
  selectAccount: PropTypes.func.isRequired,
  setAccountMode: PropTypes.func.isRequired,
  accountMode: PropTypes.string.isRequired,
}

function mapStateToProps(state){
  return {
    accountMode: financialSelectors.accountMode(state),
  }
}

function mapDispatchToProps(dispatch){
  return {
    setAccountMode: mode => dispatch(financialActions.actions.setAccountMode(mode)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(AccountContainer)