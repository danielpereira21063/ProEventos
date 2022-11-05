﻿using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersist _redeSocialPersist;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialPersist redeSocialPersist, IMapper mapper)
        {
            _redeSocialPersist = redeSocialPersist;
            _mapper = mapper;
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);

                if (redeSocial == null)
                {
                    throw new Exception("Rede social não econtrada");
                }

                _redeSocialPersist.Delete<RedeSocial>(redeSocial);

                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdAsync(palestranteId, redeSocialId);

                if (redeSocial == null)
                {
                    throw new Exception("Rede social não econtrada");
                }

                _redeSocialPersist.Delete<RedeSocial>(redeSocial);

                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);

                return _mapper.Map<RedeSocialDto[]>(redesSociais);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteAsync(int palestranteId)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);

                return _mapper.Map<RedeSocialDto[]>(redesSociais);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redesocialId)
        {
            try
            {
                var redesSocial = await _redeSocialPersist.GetRedeSocialEventoByIdAsync(eventoId, redesocialId);

                return _mapper.Map<RedeSocialDto>(redesSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redesSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdAsync(palestranteId, redeSocialId);

                return _mapper.Map<RedeSocialDto>(redesSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);

                if (redesSociais == null)
                {
                    return null;
                }

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                        continue;
                    }

                    var redeSocial = redesSociais.FirstOrDefault(x => x.Id == model.Id);
                    model.EventoId = eventoId;

                    _mapper.Map(model, redeSocial);

                    _redeSocialPersist.Update(redeSocial);

                }

                await _redeSocialPersist.SaveChangesAsync();

                var lotesRetorno = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialDto[]>(lotesRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            {
                var redesSociais = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);

                if (redesSociais == null)
                {
                    return null;
                }

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                        continue;
                    }

                    var redeSocial = redesSociais.FirstOrDefault(x => x.Id == model.Id);
                    model.PalestranteId = palestranteId;

                    _mapper.Map(model, redeSocial);

                    _redeSocialPersist.Update<RedeSocial>(redeSocial);

                }

                await _redeSocialPersist.SaveChangesAsync();

                var lotesRetorno = await _redeSocialPersist.GetAllByEventoIdAsync(palestranteId);
                return _mapper.Map<RedeSocialDto[]>(lotesRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<RedeSocialDto> AddRedeSocial(int idPalestranteOuEvento, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);

                if (isEvento)
                {
                    redeSocial.EventoId = idPalestranteOuEvento;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.PalestranteId = idPalestranteOuEvento;
                    redeSocial.EventoId = null;
                }

                _redeSocialPersist.Add(redeSocial);

                return _mapper.Map<RedeSocialDto>(redeSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
