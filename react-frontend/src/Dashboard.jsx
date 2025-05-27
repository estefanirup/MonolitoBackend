import React, { useEffect, useState } from 'react';
import sacoImg from './saco.png';

function Dashboard({ token }) {
    const [categorias, setCategorias] = useState([]);
    const [produtos, setProdutos] = useState([]);
    const [categoriaSelecionada, setCategoriaSelecionada] = useState(null);

    useEffect(() => {
        if (!token) {
            alert('Token não encontrado. Faça login.');
            return;
        }

        fetch('http://localhost:8080/api/categories', {
            headers: { Authorization: `Bearer ${token}` },
        })
            .then((res) => res.json())
            .then((data) => setCategorias(data))
            .catch((err) => console.error('Erro ao buscar categorias:', err));
    }, [token]);

    const buscarProdutos = (categoriaId) => {
        fetch(`http://localhost:8080/api/categories/${categoriaId}/products`, {
            headers: { Authorization: `Bearer ${token}` },
        })
            .then((res) => res.json())
            .then((data) => {
                setProdutos(data);
                setCategoriaSelecionada(
                    categorias.find((cat) => cat.id === categoriaId)
                );
            })
            .catch((err) => console.error('Erro ao buscar produtos:', err));
    };

    return (
        <>
            {/* Navbar */}
            <nav className="navbar navbar-expand-lg navbar-light shadow-sm fixed-top">
                <div className="container">
                    <a className="navbar-brand" href="#">
                        <img src={sacoImg} alt="Logo" />
                        <span>Minha Lojinha</span>
                    </a>
                </div>
            </nav>

            {/* Conteúdo */}
            <div className="container py-5" style={{ marginTop: '80px' }}>
                {/* Categorias */}
                <div className="text-center mb-4">
                    <h1 className="fw-bold text-pink">Nossas Categorias!</h1> 
                    <p className="text-muted">Clique na categoria para ver os produtos</p>
                </div>

                <div className="row g-4 mb-5">
                    {categorias.map((cat) => (
                        <div key={cat.id} className="col-md-4">
                            <div
                                className="card h-100 shadow-sm animate-fade-in border-pink"
                                onClick={() => buscarProdutos(cat.id)}
                                style={{ cursor: 'pointer' }}
                            >
                                <div className="card-body text-center">
                                    <h5 className="card-title text-pink">{cat.name}</h5>
                                    <p className="card-text">{cat.description}</p>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>

                {/* Produtos */}
                {categoriaSelecionada && (
                    <>
                        <div className="text-center mb-4">
                            <h2 className="fw-bold text-pink">
                                Produtos de {categoriaSelecionada.name}
                            </h2>
                        </div>

                        <div className="row g-4">
                            {produtos.map((prod) => (
                                <div key={prod.id} className="col-md-4">
                                    <div className="card h-100 shadow-sm border-light-pink">
                                        <div className="card-body">
                                            <h5 className="card-title text-pink">{prod.name}</h5>
                                            <p className="card-text">
                                                {prod.description || 'Sem descrição disponível.'}
                                            </p>
                                        </div>
                                        <div className="card-footer bg-white">
                                            <span className="fw-bold text-pink">
                                                💸 R$ {prod.price.toFixed(2)}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </>
                )}
            </div>
        </>
    );
}

export default Dashboard;
