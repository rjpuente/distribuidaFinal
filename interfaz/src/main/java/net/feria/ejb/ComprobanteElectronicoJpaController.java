/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.ejb;

import java.io.Serializable;
import java.util.Date;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Query;
import javax.persistence.EntityNotFoundException;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import net.feria.ejb.exceptions.NonexistentEntityException;
import net.feria.ejb.exceptions.PreexistingEntityException;
import net.feria.entity.ComprobanteElectronico;

/**
 *
 * @author Deth1
 */
public class ComprobanteElectronicoJpaController implements Serializable {

    public ComprobanteElectronicoJpaController(EntityManagerFactory emf) {
        this.emf = emf;
    }
    private EntityManagerFactory emf = null;

    public EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public void create(ComprobanteElectronico comprobanteElectronico) throws PreexistingEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            em.persist(comprobanteElectronico);
            em.getTransaction().commit();
        } catch (Exception ex) {
            if (findComprobanteElectronico(comprobanteElectronico.getNumeroComprobante()) != null) {
                throw new PreexistingEntityException("ComprobanteElectronico " + comprobanteElectronico + " already exists.", ex);
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void edit(ComprobanteElectronico comprobanteElectronico) throws NonexistentEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            comprobanteElectronico = em.merge(comprobanteElectronico);
            em.getTransaction().commit();
        } catch (Exception ex) {
            String msg = ex.getLocalizedMessage();
            if (msg == null || msg.length() == 0) {
                String id = comprobanteElectronico.getNumeroComprobante();
                if (findComprobanteElectronico(id) == null) {
                    throw new NonexistentEntityException("The comprobanteElectronico with id " + id + " no longer exists.");
                }
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void destroy(String id) throws NonexistentEntityException {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            ComprobanteElectronico comprobanteElectronico;
            try {
                comprobanteElectronico = em.getReference(ComprobanteElectronico.class, id);
                comprobanteElectronico.getNumeroComprobante();
            } catch (EntityNotFoundException enfe) {
                throw new NonexistentEntityException("The comprobanteElectronico with id " + id + " no longer exists.", enfe);
            }
            em.remove(comprobanteElectronico);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public List<ComprobanteElectronico> findComprobanteElectronicoEntities() {
        return findComprobanteElectronicoEntities(true, -1, -1);
    }

    public List<ComprobanteElectronico> findComprobanteElectronicoEntities(int maxResults, int firstResult) {
        return findComprobanteElectronicoEntities(false, maxResults, firstResult);
    }

    private List<ComprobanteElectronico> findComprobanteElectronicoEntities(boolean all, int maxResults, int firstResult) {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            cq.select(cq.from(ComprobanteElectronico.class));
            Query q = em.createQuery(cq);
            if (!all) {
                q.setMaxResults(maxResults);
                q.setFirstResult(firstResult);
            }
            return q.getResultList();
        } finally {
            em.close();
        }
    }

    public ComprobanteElectronico findComprobanteElectronico(String id) {
        EntityManager em = getEntityManager();
        try {
            return em.find(ComprobanteElectronico.class, id);
        } finally {
            em.close();
        }
    }

    public int getComprobanteElectronicoCount() {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            Root<ComprobanteElectronico> rt = cq.from(ComprobanteElectronico.class);
            cq.select(em.getCriteriaBuilder().count(rt));
            Query q = em.createQuery(cq);
            return ((Long) q.getSingleResult()).intValue();
        } finally {
            em.close();
        }
    }

    public List<ComprobanteElectronico> obtenerComprobantesPorFirmar() {
        EntityManager em = getEntityManager();

        Query q = em.createQuery("SELECT c FROM ComprobanteElectronico c WHERE c.transicionAutorizacion = :estado", ComprobanteElectronico.class);
        q.setParameter("estado", 7);

        return q.getResultList();
    }

    public List<ComprobanteElectronico> obtenerComprobantesPorImprimir() {
        EntityManager em = getEntityManager();

        Query q = em.createQuery("SELECT c FROM ComprobanteElectronico c WHERE c.transicionAutorizacion = :estado or c.transicionAutorizacion = :estado2", ComprobanteElectronico.class);
        q.setParameter("estado", 5);
        q.setParameter("estado2", 6);

        return q.getResultList();
    }

    public List<ComprobanteElectronico> obtenerFacturasPorFecha(Date desde, Date hasta) {
        EntityManager em = getEntityManager();

        Query q = em.createQuery("SELECT c FROM ComprobanteElectronico c WHERE c.fechaAutorizacion BETWEEN :desde AND :hasta", ComprobanteElectronico.class);
        q.setParameter("desde", desde);
        q.setParameter("hasta", hasta);

        return q.getResultList();
    }

}
