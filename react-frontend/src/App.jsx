import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Dashboard from './Dashboard';

function App({ token }) {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Dashboard token={token} />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
