const form = document.getElementById('loginForm');
const openReactBtn = document.getElementById('openReact');
let reactWindow = null;

form.addEventListener('submit', async (e) => {
  e.preventDefault();
  const username = document.getElementById('username').value;
  const password = document.getElementById('password').value;

  const response = await fetch('http://localhost:8080/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  });

  if (response.ok) {
    const data = await response.json();
    const token = data.token;
    localStorage.setItem('token', token);
    alert('Login bem-sucedido!');
    reactWindow = window.open('http://localhost:5173');

    const sendToken = () => {
      if (!reactWindow) return;
      reactWindow.postMessage({ token }, 'http://localhost:5173');
    };

    setTimeout(sendToken, 500);

  } else {
    alert('Login falhou. Verifique suas credenciais.');
  }
});

openReactBtn.addEventListener('click', () => {
  const token = localStorage.getItem('token');
  if (!token) {
    alert('Faça o login primeiro!');
    return;
  }
  reactWindow = window.open('http://localhost:5173');
  setTimeout(() => {
    if (reactWindow) {
      reactWindow.postMessage({ token }, 'http://localhost:5173');
    }
  }, 500);
});
