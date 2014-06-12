using System;
using System.Collections.Generic;
using DrArchiv.Shared.Models;
using DrArchiv.Proxy.Contracts;

namespace DrArchiv.Shared.Mappers
{
    public interface IMapper
    {
        Type PlainType { get; }
        Type DtoType { get; }
    }

    public abstract class BaseMapper<TPlain, TDto> : IMapper
                                    where TDto : BaseModel, new()
                                    where TPlain : abstractBasicTransferModel
    {
        protected BaseMapper()
        {
            PlainType = typeof (TPlain);
            DtoType = typeof (TDto);
        }

        public Type PlainType { get; private set; }

        public Type DtoType { get; private set; }

        public virtual TDto MapOne(TPlain plain)
        {
            var dto = new TDto();
            
            if (plain != null)
            {
                dto.BeginInit();

                FillDto(dto, plain);

                dto.EndInit();
            }

            return dto;
        }

        public abstract void FillDto(TDto dto, TPlain model);


        public virtual TPlain InverseMapOne(TDto dto)
        {
            return null;
        }

        public virtual List<TDto> MapMany(TPlain[] plains)
        {
            var result = new List<TDto>();

            if (plains != null)
            {
                foreach (var p in plains)
                {
                    var mapped = MapOne(p);
                    if (mapped != null)
                    {
                        result.Add(mapped);
                    }
                }
            }

            Sort(result);

            return result;
        }

        public virtual List<TPlain> InverseMapMany(IList<TDto> dtos, bool ignoreNoneSate = false)
        {
            var result = new List<TPlain>();

            if (dtos != null)
            {
                foreach (var dto in dtos)
                {
                    if (ignoreNoneSate && dto.State == ObjectStates.None) continue;

                    var mapped = InverseMapOne(dto);
                    if (mapped != null)
                    {
                        result.Add(mapped);
                    }
                }

                var trackable = dtos as TrackableCollection<TDto>;
                if (trackable != null)
                {
                    foreach (var dto in trackable.DeletedItems)
                    {
                        var mapped = InverseMapOne(dto);
                        if (mapped != null)
                        {
                            result.Add(mapped);
                        }
                    }
                }
            }

            return result;
        }

        public virtual void AddMany(TPlain[] plains, IList<TDto> list)
        {
            if (plains != null)
            {
                foreach (var p in plains)
                {
                    list.Add(MapOne(p));
                }
            }
        }

        protected virtual void Sort(List<TDto> list)
        {
        }

        protected virtual void SetState(TPlain plain, ObjectStates state)
        {
            var objectState = (int)state;
            plain.state = (stateEnum)objectState;
            plain.stateSpecified = true;
        }

        protected virtual void SetId(TPlain plain, long id)
        {
            if (id != 0)
            {
                plain.id = id;
                plain.idSpecified = true;
            }
        }

        protected virtual void SetDirty(TPlain plain, TDto dto)
        {
            if (dto == null || plain == null) return;

            plain.dirty = dto.IsDirty;
            plain.dirtySpecified = true;
        }
    }
}