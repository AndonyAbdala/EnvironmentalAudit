import { useEffect, useState } from 'react';
import './index.css';
import { getAudits, type Audit } from './services/auditService';

function App() {
  const [audits, setAudits] = useState<Audit[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadAudits();
  }, []);

  async function loadAudits() {
    try {
      const data = await getAudits();

      setAudits(data);
    } catch (error) {
      setError('No se pudieron cargar las auditorías.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="app">
      <header className="header">
        <h1>Environmental Audit POC</h1>
      </header>

      <main className="container">
        <div className="page-header">
          <h2>Auditorías</h2>

          <button className="primary-button">
            + Nueva auditoría
          </button>
        </div>

        {loading && <p>Cargando auditorías...</p>}

        {error && <p>{error}</p>}

        {!loading && !error && audits.length === 0 && (
          <p>No hay auditorías registradas.</p>
        )}

        {!loading && !error && audits.map((audit) => (
          <div className="audit-card" key={audit.id}>
            <div>
              <h3>{audit.companyName}</h3>

              <p>{audit.facilityName}</p>

              <p>
                Periodo:{' '}
                {new Date(audit.startDate).toLocaleDateString()}
                {' - '}
                {new Date(audit.endDate).toLocaleDateString()}
              </p>

              <span className="status">
                {audit.status}
              </span>
            </div>

            <div className="card-actions">
              <button>Ver</button>
              <button>PDF</button>
            </div>
          </div>
        ))}
      </main>
    </div>
  );
}

export default App;