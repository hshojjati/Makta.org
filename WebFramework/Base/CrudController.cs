using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebFramework.Base
{
    public class CrudController<TDto, TSelectDto, TEntity, TKey> : Controller
        where TDto : BaseDto<TDto, TEntity, TKey>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, TKey>, new()
        where TEntity : BaseEntity<TKey>, new()
    {
        protected readonly IRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        public CrudController(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var list = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            await FillViewDatas(cancellationToken);
            return View(list);
        }

        [HttpGet]
        public virtual async Task<ActionResult> Create(CancellationToken cancellationToken)
        {
            await FillViewDatas(cancellationToken);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(TDto dto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var model = dto.ToEntity(Mapper);

                try
                {
                    await Repository.AddAsync(model, cancellationToken);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            await FillViewDatas(cancellationToken);
            return View();
        }

        [HttpGet]
        public virtual async Task<ActionResult> Edit(TKey id, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TDto>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (dto == null)
            {
                return NotFound();
            }

            await FillViewDatas(cancellationToken, dto);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(TKey id, TDto dto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var model = await Repository.GetByIdAsync(cancellationToken, id);
                if (model == null)
                {
                    return NotFound();
                }

                try
                {
                    model = dto.ToEntity(Mapper, model);
                    await Repository.UpdateAsync(model, cancellationToken);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            await FillViewDatas(cancellationToken, dto);
            return View(dto);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Delete(TKey id, CancellationToken cancellationToken)
        {
            var model = await Repository.GetByIdAsync(cancellationToken, id);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await Repository.DeleteAsync(model, cancellationToken);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

        protected virtual async Task FillViewDatas(CancellationToken cancellationToken, TDto input = null)
        { }
    }

    public class CrudController<TDto, TSelectDto, TEntity> : CrudController<TDto, TSelectDto, TEntity, int>
        where TDto : BaseDto<TDto, TEntity, int>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity, int>, new()
        where TEntity : BaseEntity<int>, new()
    {
        public CrudController(IRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
        { }
    }

    public class CrudController<TDto, TEntity> : CrudController<TDto, TDto, TEntity, int>
        where TDto : BaseDto<TDto, TEntity, int>, new()
        where TEntity : BaseEntity<int>, new()
    {
        public CrudController(IRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
        { }
    }
}
