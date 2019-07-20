import React from 'react'
import { TextField, Button } from '@material-ui/core'
import { Flex } from 'mui-blox'
import { useSimpleField } from '../../hooks'
import { useLogin } from './hooks/useLogin'
import ActivityOverlay from '../common/ActivityOverlay'

const Login = ({ }) => {
  const passwordField = useSimpleField('Password')
  const { tryLogin, isActive, error } = useLogin()

  const doLogin = () => tryLogin(passwordField.value)

  return (
    <Flex flexFull alignItems='center'>
      <Flex flexDirection='column'>
        <ActivityOverlay isActive={isActive}>
          <TextField
            {...passwordField}
            type='password'
            onKeyPress={({ key }) => key === 'Enter' && doLogin()}
            error={error}
            helperText={error}
          />
          <Button onClick={doLogin} variant='contained' color='primary'>Login</Button>
        </ActivityOverlay>
      </Flex>
    </Flex>
  )
}

export default Login
