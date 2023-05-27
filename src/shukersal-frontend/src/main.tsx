import React from 'react'
import ReactDOM from 'react-dom/client'

import { store } from './redux/store';
import { Provider } from 'react-redux'

import { RouterProvider } from 'react-router-dom';

import { ThemeProvider } from '@mui/material'

import { theme } from './theme'
import { router } from './router';

import './index.css'


ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <Provider store={store}>
      <ThemeProvider theme={theme}>
          <RouterProvider router={router} />
      </ThemeProvider>
    </Provider>
  </React.StrictMode>,
)
