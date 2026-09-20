import { useEffect, useState } from 'react';

import {
  getAudit,
  calculateAudit,
  type Audit,
  type CalculateAuditRequest,
  type AuditCalculationResult
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
    const [waterConsumptionM3, setWaterConsumptionM3] =
    useState('');
    const [wastewaterM3, setWastewaterM3] =
    useState('');
    const [hazardousWasteKg, setHazardousWasteKg] =
    useState('');
    const [nonHazardousWasteKg, setNonHazardousWasteKg] =
    useState('');
    const [recycledWasteKg, setRecycledWasteKg] =
    useState('');
    const [dieselLiters, setDieselLiters] =
    useState('');
    const [gasolineLiters, setGasolineLiters] =
    useState('');
    const [result, setResult] =
  useState<AuditCalculationResult | null>(null);

const [calculating, setCalculating] =
  useState(false);
  const [calculationError, setCalculationError] =
  useState<string | null>(null);

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

  async function handleCalculate() {

  setCalculationError(null);
  setCalculating(true);

  try {

    const request: CalculateAuditRequest = {
      electricityKwh: Number(electricityKwh),
      naturalGasM3: Number(naturalGasM3),

      waterConsumptionM3:
        Number(waterConsumptionM3),

      wastewaterM3:
        Number(wastewaterM3),

      hazardousWasteKg:
        Number(hazardousWasteKg),

      nonHazardousWasteKg:
        Number(nonHazardousWasteKg),

      recycledWasteKg:
        Number(recycledWasteKg),

      dieselLiters:
        Number(dieselLiters),

      gasolineLiters:
        Number(gasolineLiters)
    };

    const calculation =
      await calculateAudit(
        auditId,
        request
      );

    setResult(calculation);

  } catch {

    setCalculationError(
      'No se pudo calcular la auditoría.'
    );

  } finally {

    setCalculating(false);

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
                value={waterConsumptionM3}
                onChange={(e) =>
                    setWaterConsumptionM3(e.target.value)
                }
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
                value={wastewaterM3}
                onChange={(e) =>
                    setWastewaterM3(e.target.value)
                }
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
  value={hazardousWasteKg}
  onChange={(e) =>
    setHazardousWasteKg(e.target.value)
  }
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
  value={nonHazardousWasteKg}
  onChange={(e) =>
    setNonHazardousWasteKg(e.target.value)
  }
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
  value={recycledWasteKg}
  onChange={(e) =>
    setRecycledWasteKg(e.target.value)
  }
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
  value={dieselLiters}
  onChange={(e) =>
    setDieselLiters(e.target.value)
  }
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
  value={gasolineLiters}
  onChange={(e) =>
    setGasolineLiters(e.target.value)
  }
  placeholder="800"
/>

    <span>L</span>
  </div>

</div>

      <button
      className="primary-button calculate-button"
      onClick={handleCalculate}
      disabled={calculating}
    >
      {calculating
        ? 'Calculando...'
        : 'Calcular auditoría'}
    </button>

    {calculationError && (
      <p className="error">
        {calculationError}
      </p>
    )}

    {result && (
      <div className="result-card">

        <h2>
          Auditoría completada ✓
        </h2>

        <div className="score">
          {result.score.toFixed(1)}
          <span>/ 100</span>
        </div>

        <div className="result-grid">

          <div>
            <span>Emisiones</span>
            <strong>
              {result.totalEmissions.toFixed(2)}
            </strong>
          </div>

          <div>
            <span>Residuos totales</span>
            <strong>
              {result.totalWaste.toFixed(2)} kg
            </strong>
          </div>

          <div>
            <span>Tasa de reciclaje</span>
            <strong>
              {result.recyclingRate.toFixed(2)} %
            </strong>
          </div>

        </div>

        <button className="primary-button">
          Descargar PDF
        </button>

      </div>
    )}

    </div>
  );

  


  
}

export default AuditDetail;