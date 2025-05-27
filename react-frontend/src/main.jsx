// main.jsx
import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.jsx';

function Root() {
  const [token, setToken] = useState(() => {
    return localStorage.getItem('token') || null;
  });

  useEffect(() => {
    function handleMessage(event) {
      if (event.origin !== 'http://localhost:5500') return;

      const { token: receivedToken } = event.data;
      if (receivedToken) {
        localStorage.setItem('token', receivedToken);
        setToken(receivedToken);
        console.log('Token recebido e armazenado.');
      }
    }

    window.addEventListener('message', handleMessage);

    return () => {
      window.removeEventListener('message', handleMessage);
    };
  }, []);

  return <App token={token} />;
}

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <Root />
  </React.StrictMode>
);
