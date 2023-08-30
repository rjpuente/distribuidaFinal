/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.ejb;

import java.io.Serializable;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Query;
import javax.persistence.EntityNotFoundException;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import net.feria.ejb.exceptions.NonexistentEntityException;
import net.feria.entity.InformacionFiscal;

/**
 *
 * @author Deth1
 */
public class InformacionFiscalJpaController implements Serializable {

    public InformacionFiscalJpaController(EntityManagerFactory emf) {
        this.emf = emf;
    }
    private EntityManagerFactory emf = null;

    public EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public void create(InformacionFiscal informacionFiscal) {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            em.persist(informacionFiscal);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void edit(InformacionFiscal informacionFiscal) throws NonexistentEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            informacionFiscal = em.merge(informacionFiscal);
            em.getTransaction().commit();
        } catch (Exception ex) {
            String msg = ex.getLocalizedMessage();
            if (msg == null || msg.length() == 0) {
                Integer id = informacionFiscal.getCodigoInformacionFiscal();
                if (findInformacionFiscal(id) == null) {
                    throw new NonexistentEntityException("The informacionFiscal with id " + id + " no longer exists.");
                }
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void destroy(Integer id) throws NonexistentEntityException {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            InformacionFiscal informacionFiscal;
            try {
                informacionFiscal = em.getReference(InformacionFiscal.class, id);
                informacionFiscal.getCodigoInformacionFiscal();
            } catch (EntityNotFoundException enfe) {
                throw new NonexistentEntityException("The informacionFiscal with id " + id + " no longer exists.", enfe);
            }
            em.remove(informacionFiscal);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public List<InformacionFiscal> findInformacionFiscalEntities() {
        return findInformacionFiscalEntities(true, -1, -1);
    }

    public List<InformacionFiscal> findInformacionFiscalEntities(int maxResults, int firstResult) {
        return findInformacionFiscalEntities(false, maxResults, firstResult);
    }

    private List<InformacionFiscal> findInformacionFiscalEntities(boolean all, int maxResults, int firstResult) {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            cq.select(cq.from(InformacionFiscal.class));
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

    public InformacionFiscal findInformacionFiscal(Integer id) {
        EntityManager em = getEntityManager();
        try {
            return em.find(InformacionFiscal.class, id);
        } finally {
            em.close();
        }
    }

    public int getInformacionFiscalCount() {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            Root<InformacionFiscal> rt = cq.from(InformacionFiscal.class);
            cq.select(em.getCriteriaBuilder().count(rt));
            Query q = em.createQuery(cq);
            return ((Long) q.getSingleResult()).intValue();
        } finally {
            em.close();
        }
    }
    
}
