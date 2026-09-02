import { useState } from 'react';
import { createAudit, type CreateAuditRequest } from '../services/auditService';

interface Props {
  onCreated: () => void;
  onCancel: () => void;
}

function CreateAuditForm({ onCreated, onCancel }: Props) {
  const [form, setForm] = useState<CreateAuditRequest>({
    companyName: '',
    facilityName: '',
    responsible: '',
    startDate: '',
    endDate: ''
  });

  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  function handleChange(
    event: React.ChangeEvent<HTMLInputElement>
  ) {
    const { name, value } = event.target;

    setForm({
      ...form,
      [name]: value
    });
  }

  async function handleSubmit(
    event: React.FormEvent
  ) {
    event.preventDefault();

    setError(null);
    setLoading(true);

    try {
      await createAudit(form);

      onCreated();
    } catch {
      setError('No se pudo crear la auditoría.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="form-container">
      <h2>Nueva auditoría</h2>

      {error && (
        <p className="error">
          {error}
        </p>
      )}

      <form onSubmit={handleSubmit}>

        <div className="form-group">
          <label>Empresa</label>

          <input
            name="companyName"
            value={form.companyName}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Planta</label>

          <input
            name="facilityName"
            value={form.facilityName}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Responsable</label>

          <input
            name="responsible"
            value={form.responsible}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Fecha de inicio</label>

          <input
            type="date"
            name="startDate"
            value={form.startDate}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Fecha de finalización</label>

          <input
            type="date"
            name="endDate"
            value={form.endDate}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-actions">
          <button
            type="button"
            onClick={onCancel}
          >
            Cancelar
          </button>

          <button
            type="submit"
            className="primary-button"
            disabled={loading}
          >
            {loading ? 'Creando...' : 'Crear auditoría'}
          </button>
        </div>

      </form>
    </div>
  );
}

export default CreateAuditForm;