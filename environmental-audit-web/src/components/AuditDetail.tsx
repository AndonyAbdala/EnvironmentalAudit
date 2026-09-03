import { useEffect, useState } from 'react';
import {
  getAudit,
  type Audit
} from '../services/auditService';

interface Props {
  auditId: string;
  onBack: () => void;
}

function AuditDetail({ auditId, onBack }: Props) {
    const [audit, setAudit] = useState<Audit | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [electricityKwh, setElectricityKwh] =
    useState('');
    const [naturalGasM3, setNaturalGasM3] =
    useState('');

  useEffect(() => {
    loadAudit();
  }, [auditId]);

  async function loadAudit() {
    try {
      setLoading(true);

      const data = await getAudit(auditId);

      setAudit(data);
    } catch {
      setError(
        'No se pudo cargar la auditoría.'
      );
    } finally {
      setLoading(false);
    }
  }

  if (loading) {
    return <p>Cargando auditoría...</p>;
  }

  if (error) {
    return <p className="error">{error}</p>;
  }

  if (!audit) {
    return <p>Auditoría no encontrada.</p>;
  }

  return (
    <div>

      <button onClick={onBack}>
        ← Volver
      </button>

      <div className="audit-detail-header">

        <h2>{audit.companyName}</h2>

        <p>{audit.facilityName}</p>

        <p>
          Responsable: {audit.responsible}
        </p>

        <p>
          Periodo:{' '}
          {new Date(
            audit.startDate
          ).toLocaleDateString()}
          {' - '}
          {new Date(
            audit.endDate
          ).toLocaleDateString()}
        </p>

        <span className="status">
          {audit.status}
        </span>

      </div>

      <div className="tabs">

        <button className="active-tab">
          Energía
        </button>

        <button>
          Agua
        </button>

        <button>
          Residuos
        </button>

        <button>
          Combustibles
        </button>

      </div>

      <div className="environmental-form">

        <h3>Energía</h3>

        <div className="form-group">
          <label>
            Consumo eléctrico
          </label>

          <input
            type="number"
            value={electricityKwh}
            onChange={(e) =>
                setElectricityKwh(e.target.value)
            }
            placeholder="125000"
          />

          <span>kWh</span>
        </div>

        <div className="form-group">
          <label>
            Consumo de gas natural
          </label>

          <input
            type="number"
            value={naturalGasM3}
            onChange={(e) =>
                setNaturalGasM3(e.target.value)
            }
            placeholder="8500"
            />

          <span>m³</span>
        </div>

      </div>

      <div className="environmental-form">

        <h3>Agua</h3>

        <div className="form-group">
            <label>
            Agua utilizada
            </label>

            <input
            type="number"
            placeholder="1250"
            />

            <span>m³</span>
        </div>

        <div className="form-group">
            <label>
            Agua residual
            </label>

            <input
            type="number"
            placeholder="900"
            />

            <span>m³</span>
        </div>

      </div>

      <div className="environmental-form">

  <h3>Residuos</h3>

  <div className="form-group">
    <label>
      Residuos peligrosos
    </label>

    <input
      type="number"
      placeholder="120"
    />

    <span>kg</span>
  </div>

  <div className="form-group">
    <label>
      Residuos no peligrosos
    </label>

    <input
      type="number"
      placeholder="850"
    />

    <span>kg</span>
  </div>

  <div className="form-group">
    <label>
      Residuos reciclados
    </label>

    <input
      type="number"
      placeholder="500"
    />

    <span>kg</span>
  </div>

</div>

<div className="environmental-form">

  <h3>Combustibles</h3>

  <div className="form-group">
    <label>
      Diesel
    </label>

    <input
      type="number"
      placeholder="2500"
    />

    <span>L</span>
  </div>

  <div className="form-group">
    <label>
      Gasolina
    </label>

    <input
      type="number"
      placeholder="800"
    />

    <span>L</span>
  </div>

</div>

      <button className="primary-button calculate-button">
        Calcular auditoría
      </button>

    </div>
  );
}

export default AuditDetail;