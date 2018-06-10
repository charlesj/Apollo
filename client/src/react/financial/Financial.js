import React, { Component, } from 'react'
import PropTypes from 'prop-types'
import { connect, } from 'react-redux'
import {
  Page,
  Container,
  FlexRow,
  FlexContainer,
  AddButton,
} from '../_controls'
import SelectList from '../_controls/SelectListWithTools'
import { financialSelectors, } from '../../redux/selectors'
import { financialActions, } from '../../redux/actions'
import AccountContainer from './AccountContainer'

import './financial.css'

class Finance extends Component {
  componentDidMount() {
    this.props.loadAccounts()
  }

  render() {
    const {
      accounts,
      selectAccount,
      selectedAccount,
      saveAccount,
    } = this.props
    return (
      <Page>
        <FlexRow>
          <FlexContainer>
            <AddButton
              noun="account"
              onClick={() => selectAccount({id: 'new',})}
            />
            <Container width={200}>
              <SelectList
                items={accounts}
                onSelectItem={(account) => selectAccount(account)}
                selectedItem={selectedAccount}
                labelField="name"
              />
            </Container>
          </FlexContainer>
          {selectedAccount && (
            <FlexContainer>
              <AccountContainer
                selectedAccount={selectedAccount}
                saveAccount={saveAccount}
                selectAccount={() => selectAccount(null)}
              />
            </FlexContainer>)}
          <FlexContainer>graphs</FlexContainer>
        </FlexRow>
      </Page>
    )
  }
}

Finance.propTypes = {
  accounts: PropTypes.array.isRequired,
  selectedAccount: PropTypes.object,
  loadAccounts: PropTypes.func.isRequired,
  saveAccount: PropTypes.func.isRequired,
  selectAccount: PropTypes.func.isRequired,
}

function mapStateToProps(state){
  return {
    accounts: financialSelectors.accounts(state),
    selectedAccount: financialSelectors.selectedAccount(state),
  }
}

function mapDispatchToProps(dispatch){
  return {
    loadAccounts: () => dispatch(financialActions.loadAccounts()),
    saveAccount: account => dispatch(financialActions.saveAccount(account)),
    selectAccount: account => dispatch(financialActions.selectAccount(account)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Finance)
