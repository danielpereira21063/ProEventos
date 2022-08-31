﻿using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly ILotePersist _lotePersist;
        private readonly IGeralPersist _geralPersist;
        private readonly IMapper _mapper;

        public LoteService(ILotePersist lotePersist, IGeralPersist geralPersist, IMapper mapper)
        {
            _lotePersist = lotePersist;
            _geralPersist = geralPersist;
            _mapper = mapper;
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);

                if (lote == null)
                {
                    throw new Exception("Lote para delete não encontrado.");
                }

                _geralPersist.Delete<Lote>(lote);

                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIds(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);

                if (lote == null)
                {
                    return null;
                }

                return _mapper.Map<LoteDto>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetAllLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(lotes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);

                lote.EventoId = eventoId;

                _geralPersist.Add(lote);

                return _mapper.Map<LoteDto>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetAllLotesByEventoIdAsync(eventoId);

                if (lotes == null)
                {
                    return null;
                }

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                        continue;
                    }

                    var lote = lotes.FirstOrDefault(x => x.Id == model.Id);
                    model.EventoId = eventoId;

                    _mapper.Map(model, lote);

                    _geralPersist.Update<Lote>(lote);

                }

                await _geralPersist.SaveChangesAsync();

                var lotesRetorno = await _lotePersist.GetAllLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(lotesRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
